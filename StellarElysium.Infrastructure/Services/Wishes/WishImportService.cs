using System.Globalization;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Wishes.Import;
using StellarElysium.Domain.Entities.Wishes;
using StellarElysium.Infrastructure.Mappers.Wishes;

namespace StellarElysium.Infrastructure.Services.Wishes;

public sealed class WishImportService(
    IGenshinAccountRepository accountRepository,
    IWishRepository wishRepository,
    WishesMapper mapper,
    ILogger<WishImportService> logger) : IWishImportService
{
    public async Task<WishImportResponse> ImportAsync(
        WishImportRequest request,
        CancellationToken cancellationToken)
    {
        var totalReceived = request.Wishes.Count;
        logger.LogInformation("Wish import started. Uid {Uid} Total {Total}", request.Account.Uid, totalReceived);

        var account = await accountRepository.GetByUidAsync(request.Account.Uid, request.Account.Server, cancellationToken);
        if (account is null)
        {
            account = mapper.Map(request.Account);
            account = await accountRepository.CreateAsync(account, cancellationToken);
            logger.LogInformation("Account created. AccountId {AccountId}", account.Id);
        }

        var convertedWishes = new List<Wish>(totalReceived);
        var invalid = 0;

        foreach (var item in request.Wishes)
        {
            if (!TryMap(item, account.Id, out var wish))
            {
                invalid++;
                continue;
            }

            convertedWishes.Add(wish);
        }

        if (convertedWishes.Count == 0)
        {
            logger.LogInformation("Wish import finished without valid wishes.");
            return new WishImportResponse
            {
                GenshinAccountId = account.Id,
                TotalReceived = totalReceived,
                Imported = 0,
                Ignored = totalReceived
            };
        }

        var pullIds = convertedWishes
            .Select(wish => wish.PullId)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var existing = await wishRepository.GetExistingPullIdsAsync(account.Id, pullIds, cancellationToken);
        var newItems = new List<Wish>();
        var seen = new HashSet<string>(existing, StringComparer.Ordinal);

        foreach (var wish in convertedWishes)
        {
            if (!seen.Add(wish.PullId))
            {
                continue;
            }

            newItems.Add(wish);
        }

        await wishRepository.AddAsync(newItems, cancellationToken);
        await wishRepository.SaveAsync(cancellationToken);

        var ignored = totalReceived - newItems.Count;
        if (invalid > 0)
        {
            logger.LogWarning("Wishes invalid ignored: {Total}", invalid);
        }

        logger.LogInformation("Wish import finished. New {New} Ignored {Ignored}", newItems.Count, ignored);

        return new WishImportResponse
        {
            GenshinAccountId = account.Id,
            TotalReceived = totalReceived,
            Imported = newItems.Count,
            Ignored = ignored
        };
    }

    private static bool TryMap(WishImportItemRequest item, Guid accountId, out Wish wish)
    {
        wish = new Wish();

        if (string.IsNullOrWhiteSpace(item.PullId))
        {
            return false;
        }

        if (!int.TryParse(item.GachaType, NumberStyles.Integer, CultureInfo.InvariantCulture, out var gachaType))
        {
            return false;
        }

        if (!int.TryParse(item.RankType, NumberStyles.Integer, CultureInfo.InvariantCulture, out var rankType))
        {
            return false;
        }

        if (!int.TryParse(item.Quantity, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity))
        {
            return false;
        }

        if (!DateTime.TryParseExact(
                item.WishTime,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var wishTime))
        {
            if (!DateTime.TryParse(item.WishTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out wishTime))
            {
                return false;
            }
        }

        wish = new Wish
        {
            GenshinAccountId = accountId,
            PullId = item.PullId,
            GachaType = gachaType,
            ItemId = item.ItemId,
            ItemName = item.ItemName,
            ItemType = item.ItemType,
            RankType = rankType,
            Quantity = quantity,
            WishTime = wishTime
        };

        return true;
    }
}
