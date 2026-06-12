using StellarElysium.Domain.Dtos.Wishes.Import;

namespace StellarElysium.Application.Interfaces.Services.Wishes;

public interface IWishImportService
{
    Task<WishImportResponse> ImportAsync(WishImportRequest request, CancellationToken cancellationToken);
}
