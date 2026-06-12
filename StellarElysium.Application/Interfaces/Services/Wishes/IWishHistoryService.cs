using StellarElysium.Domain.Dtos.Wishes.Import;

namespace StellarElysium.Application.Interfaces.Services.Wishes;

public interface IWishHistoryService
{
    Task<IReadOnlyList<WishImportItemRequest>> FetchAsync(
        string url,
        IReadOnlyCollection<int> gachaTypes,
        CancellationToken cancellationToken);
}
