using StellarElysium.Domain.Entities.Characters;
using StellarElysium.Domain.Utils;

namespace StellarElysium.Application.Interfaces.Repositories.Characters;

public interface ICharacterRepository
{
    Task<PaginatedResult<Character>> ListAsync(
        IReadOnlyCollection<string> names,
        string? search,
        bool includeDeleted,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<List<Character>> GetByIdsOrNamesAsync(
        IReadOnlyCollection<Guid> ids,
        IReadOnlyCollection<string> names,
        bool includeDeleted,
        CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<Character> characters, CancellationToken cancellationToken);
    void RemoveRange(IEnumerable<Character> characters);
    Task SaveAsync(CancellationToken cancellationToken);
}
