using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Dtos.Wishes.Query;
using StellarElysium.Infrastructure.Mappers.Wishes;

namespace StellarElysium.Infrastructure.Services.Wishes;

public sealed class WishQueryService(
    IWishRepository wishRepository,
    WishesMapper mapper,
    ILogger<WishQueryService> logger) : IWishQueryService
{
    public async Task<PaginatedResponse<WishResponse>> ListAsync(
        WishQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Listing wishes. AccountId {AccountId}", request.GenshinAccountId);

        var skip = request.Skip < 0 ? 0 : request.Skip;
        var take = request.Take <= 0 ? 20 : request.Take;
        if (take > 200)
        {
            take = 200;
        }

        var result = await wishRepository.GetByFilterAsync(
            request.GenshinAccountId,
            request.GachaType,
            request.StartDate,
            request.EndDate,
            request.RankType,
            request.Name,
            request.ItemType,
            request.SortBy,
            request.Direction,
            skip,
            take,
            cancellationToken);

        return new PaginatedResponse<WishResponse>
        {
            Total = result.Total,
            Skip = skip,
            Take = take,
            Items = result.Items.Select(mapper.Map).ToList()
        };
    }
}
