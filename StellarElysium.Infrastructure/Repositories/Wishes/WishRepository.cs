using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Domain.Entities.Wishes;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Enums.Wishes;
using StellarElysium.Domain.Utils;
using StellarElysium.Infrastructure.Persistence;

namespace StellarElysium.Infrastructure.Repositories.Wishes;

public sealed class WishRepository(
    AppDbContext context,
    ILogger<WishRepository> logger) : IWishRepository
{
    public async Task<HashSet<string>> GetExistingPullIdsAsync(
        Guid genshinAccountId,
        IEnumerable<string> pullIds,
        CancellationToken cancellationToken)
    {
        var list = pullIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (list.Count == 0)
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        var existing = await context.Wishes
            .AsNoTracking()
            .Where(wish => wish.GenshinAccountId == genshinAccountId && list.Contains(wish.PullId))
            .Select(wish => wish.PullId)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Existing pull ids found: {Total}", existing.Count);

        return existing.ToHashSet(StringComparer.Ordinal);
    }

    public Task AddAsync(IEnumerable<Wish> wishes, CancellationToken cancellationToken)
    {
        var list = wishes.ToList();
        if (list.Count == 0)
        {
            return Task.CompletedTask;
        }

        logger.LogInformation("Adding {Total} wishes.", list.Count);
        context.Wishes.AddRange(list);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Wish>> GetByAccountAsync(
        Guid genshinAccountId,
        int? gachaType,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = context.Wishes
            .AsNoTracking()
            .Where(wish => wish.GenshinAccountId == genshinAccountId);

        if (gachaType.HasValue)
        {
            query = query.Where(wish => wish.GachaType == gachaType.Value);
        }

        return await ApplyPagination(query, skip, take)
            .OrderByDescending(wish => wish.WishTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Wish>> GetByAccountAsync(Guid genshinAccountId, CancellationToken cancellationToken)
    {
        return await context.Wishes
            .AsNoTracking()
            .Where(wish => wish.GenshinAccountId == genshinAccountId)
            .OrderByDescending(wish => wish.WishTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<Wish>> GetByFilterAsync(
        Guid genshinAccountId,
        int? gachaType,
        DateTime? startDate,
        DateTime? endDate,
        int? rankType,
        string? name,
        string? itemType,
        WishSortField? sortBy,
        SortDirection direction,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = context.Wishes
            .AsNoTracking()
            .Where(wish => wish.GenshinAccountId == genshinAccountId);

        if (gachaType.HasValue)
        {
            query = query.Where(wish => wish.GachaType == gachaType.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(wish => wish.WishTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(wish => wish.WishTime <= endDate.Value);
        }

        if (rankType.HasValue)
        {
            query = query.Where(wish => wish.RankType == rankType.Value);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var term = $"%{name.Trim()}%";
            query = query.Where(wish => EF.Functions.Like(wish.ItemName, term));
        }

        if (!string.IsNullOrWhiteSpace(itemType))
        {
            var term = $"%{itemType.Trim()}%";
            query = query.Where(wish => EF.Functions.Like(wish.ItemType, term));
        }

        var total = await query.CountAsync(cancellationToken);
        var sorted = ApplySorting(query, sortBy ?? WishSortField.WishTime, direction);
        var items = await ApplyPagination(sorted, skip, take)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Wish>
        {
            Total = total,
            Items = items
        };
    }

    private static IQueryable<Wish> ApplyPagination(IQueryable<Wish> query, int skip, int take)
    {
        if (skip < 0)
        {
            skip = 0;
        }

        if (take <= 0)
        {
            take = 20;
        }

        if (take > 200)
        {
            take = 200;
        }

        return query.Skip(skip).Take(take);
    }

    private static IQueryable<Wish> ApplySorting(
        IQueryable<Wish> query,
        WishSortField sortBy,
        SortDirection direction)
    {
        if (sortBy == WishSortField.RankType)
        {
            return direction == SortDirection.Asc
                ? query.OrderBy(wish => wish.RankType)
                : query.OrderByDescending(wish => wish.RankType);
        }

        if (sortBy == WishSortField.ItemName)
        {
            return direction == SortDirection.Asc
                ? query.OrderBy(wish => wish.ItemName)
                : query.OrderByDescending(wish => wish.ItemName);
        }

        if (sortBy == WishSortField.GachaType)
        {
            return direction == SortDirection.Asc
                ? query.OrderBy(wish => wish.GachaType)
                : query.OrderByDescending(wish => wish.GachaType);
        }

        return direction == SortDirection.Asc
            ? query.OrderBy(wish => wish.WishTime)
            : query.OrderByDescending(wish => wish.WishTime);
    }
}
