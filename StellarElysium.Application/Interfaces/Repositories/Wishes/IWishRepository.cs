using StellarElysium.Domain.Entities.Wishes;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Enums.Wishes;
using StellarElysium.Domain.Utils;

namespace StellarElysium.Application.Interfaces.Repositories.Wishes;

public interface IWishRepository
{
    Task<HashSet<string>> GetExistingPullIdsAsync(Guid genshinAccountId, IEnumerable<string> pullIds, CancellationToken cancellationToken);
    Task<List<Wish>> GetByAccountAsync(Guid genshinAccountId, int? gachaType, int skip, int take, CancellationToken cancellationToken);
    Task<List<Wish>> GetByAccountAsync(Guid genshinAccountId, CancellationToken cancellationToken);
    Task<PaginatedResult<Wish>> GetByFilterAsync(
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
        CancellationToken cancellationToken);
    Task AddAsync(IEnumerable<Wish> wishes, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
