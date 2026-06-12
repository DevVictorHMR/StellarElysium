using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Dtos.Wishes.Query;

namespace StellarElysium.Application.Interfaces.Services.Wishes;

public interface IWishQueryService
{
    Task<PaginatedResponse<WishResponse>> ListAsync(
        WishQueryRequest request,
        CancellationToken cancellationToken);
}
