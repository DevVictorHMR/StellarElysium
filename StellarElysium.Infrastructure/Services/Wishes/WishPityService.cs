using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Wishes.Query;

namespace StellarElysium.Infrastructure.Services.Wishes;

public sealed class WishPityService(
    IWishRepository wishRepository,
    ILogger<WishPityService> logger) : IWishPityService
{
    public async Task<PitySummaryResponse> GetPityAsync(Guid genshinAccountId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calculating pity summary. AccountId {AccountId}", genshinAccountId);

        var wishes = await wishRepository.GetByAccountAsync(genshinAccountId, cancellationToken);
        var groups = wishes
            .GroupBy(wish => wish.GachaType)
            .OrderBy(group => group.Key)
            .ToList();

        var response = new PitySummaryResponse
        {
            GenshinAccountId = genshinAccountId
        };

        foreach (var group in groups)
        {
            var ordered = group
                .OrderByDescending(wish => wish.WishTime)
                .ToList();

            var fiveStarPity = 0;
            DateTime? lastFiveStar = null;
            string? lastFiveStarName = null;
            foreach (var wish in ordered)
            {
                if (wish.RankType == 5)
                {
                    lastFiveStar = wish.WishTime;
                    lastFiveStarName = wish.ItemName;
                    break;
                }

                fiveStarPity++;
            }

            var fourStarPity = 0;
            DateTime? lastFourStar = null;
            string? lastFourStarName = null;
            foreach (var wish in ordered)
            {
                if (wish.RankType == 4)
                {
                    lastFourStar = wish.WishTime;
                    lastFourStarName = wish.ItemName;
                    break;
                }

                fourStarPity++;
            }

            response.Banners.Add(new PityBannerResponse
            {
                GachaType = group.Key,
                Total = ordered.Count,
                FiveStarPity = ordered.Count == 0 ? 0 : fiveStarPity,
                FourStarPity = ordered.Count == 0 ? 0 : fourStarPity,
                LastFiveStar = lastFiveStar,
                LastFourStar = lastFourStar,
                LastFiveStarName = lastFiveStarName,
                LastFourStarName = lastFourStarName
            });
        }

        return response;
    }

    public async Task<PityDetailResponse> GetPityDetailAsync(PityDetailRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calculating pity detail. AccountId {AccountId}", request.GenshinAccountId);

        var wishes = await wishRepository.GetByAccountAsync(request.GenshinAccountId, cancellationToken);
        var query = wishes.AsEnumerable();

        if (request.GachaType.HasValue)
        {
            query = query.Where(wish => wish.GachaType == request.GachaType.Value);
        }

        var grouped = query
            .GroupBy(wish => wish.GachaType)
            .OrderBy(group => group.Key)
            .ToList();

        var response = new PityDetailResponse
        {
            GenshinAccountId = request.GenshinAccountId
        };

        foreach (var group in grouped)
        {
            var ordered = group
                .OrderByDescending(wish => wish.WishTime)
                .ToList();

            var fiveStarPity = 0;
            foreach (var wish in ordered)
            {
                if (wish.RankType == 5)
                {
                    break;
                }

                fiveStarPity++;
            }

            var fourStarPity = 0;
            foreach (var wish in ordered)
            {
                if (wish.RankType == 4)
                {
                    break;
                }

                fourStarPity++;
            }

            var limit = Math.Clamp(request.QuantityItems, 1, 20);

            var lastFiveStars = ordered
                .Where(wish => wish.RankType == 5)
                .Take(limit)
                .Select(wish => new PityItemResponse
                {
                    ItemName = wish.ItemName,
                    RankType = wish.RankType,
                    WishTime = wish.WishTime
                })
                .ToList();

            var lastFourStars = ordered
                .Where(wish => wish.RankType == 4)
                .Take(limit)
                .Select(wish => new PityItemResponse
                {
                    ItemName = wish.ItemName,
                    RankType = wish.RankType,
                    WishTime = wish.WishTime
                })
                .ToList();

            response.Banners.Add(new PityBannerDetailResponse
            {
                GachaType = group.Key,
                FiveStarPity = ordered.Count == 0 ? 0 : fiveStarPity,
                FourStarPity = ordered.Count == 0 ? 0 : fourStarPity,
                LastFiveStars = lastFiveStars,
                LastFourStars = lastFourStars
            });
        }

        return response;
    }
}
