using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Characters;
using StellarElysium.Domain.Entities.Characters;
using StellarElysium.Domain.Utils;
using StellarElysium.Infrastructure.Persistence;

namespace StellarElysium.Infrastructure.Repositories.Characters;

public sealed class CharacterRepository(
    AppDbContext context,
    ILogger<CharacterRepository> logger) : ICharacterRepository
{
    public async Task<PaginatedResult<Character>> ListAsync(
        IReadOnlyCollection<string> names,
        string? search,
        bool includeDeleted,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = BuildBaseQuery(includeDeleted);

        if (names.Count > 0)
        {
            query = query.Where(character => names.Contains(character.Name));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(character =>
                EF.Functions.Like(character.Name, term) ||
                (character.GameCode != null && EF.Functions.Like(character.GameCode, term)) ||
                EF.Functions.Like(character.ProfileLocalizationsJson, term));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(character => character.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Character>
        {
            Total = total,
            Items = items
        };
    }

    public async Task<List<Character>> GetByIdsOrNamesAsync(
        IReadOnlyCollection<Guid> ids,
        IReadOnlyCollection<string> names,
        bool includeDeleted,
        CancellationToken cancellationToken)
    {
        if (ids.Count == 0 && names.Count == 0)
        {
            return new List<Character>();
        }

        return await BuildBaseQuery(includeDeleted, asNoTracking: false)
            .Where(character => ids.Contains(character.Id) || names.Contains(character.Name))
            .ToListAsync(cancellationToken);
    }

    public Task AddRangeAsync(IEnumerable<Character> characters, CancellationToken cancellationToken)
    {
        var list = characters.ToList();
        if (list.Count == 0)
        {
            return Task.CompletedTask;
        }

        logger.LogInformation("Adding {Total} characters.", list.Count);
        context.Characters.AddRange(list);
        return Task.CompletedTask;
    }

    public void RemoveRange(IEnumerable<Character> characters)
    {
        var list = characters.ToList();
        if (list.Count == 0)
        {
            return;
        }

        logger.LogInformation("Removing {Total} characters.", list.Count);
        context.Characters.RemoveRange(list);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Character> BuildBaseQuery(bool includeDeleted, bool asNoTracking = true)
    {
        var query = asNoTracking ? context.Characters.AsNoTracking() : context.Characters;

        if (!includeDeleted)
        {
            query = query.Where(character => !character.IsDeleted);
        }

        return query;
    }
}
