using StellarElysium.Domain.Dtos.Wishes.Import;

namespace StellarElysium.Application.Interfaces.Services.Wishes;

public interface IWishHistoryUrlImportService
{
    Task<WishImportResponse> ImportByUrlAsync(
        WishUrlImportRequest request,
        CancellationToken cancellationToken);
}
