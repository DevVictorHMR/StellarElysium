using StellarElysium.Domain.Dtos.Wishes.Query;

namespace StellarElysium.Application.Interfaces.Services.Wishes;

public interface IWishPityService
{
    Task<PitySummaryResponse> GetPityAsync(Guid genshinAccountId, CancellationToken cancellationToken);

    Task<PityDetailResponse> GetPityDetailAsync(PityDetailRequest request, CancellationToken cancellationToken);
}
