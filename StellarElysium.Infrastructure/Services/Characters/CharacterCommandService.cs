using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Providers;
using StellarElysium.Application.Interfaces.Repositories.Characters;
using StellarElysium.Application.Interfaces.Services.Characters;
using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;
using StellarElysium.Domain.Enums.Characters;
using StellarElysium.Infrastructure.Mappers.Characters;

namespace StellarElysium.Infrastructure.Services.Characters;

public sealed class CharacterCommandService(
    ICharacterRepository characterRepository,
    IDateTimeProvider dateTimeProvider,
    CharactersMapper mapper,
    ILogger<CharacterCommandService> logger) : ICharacterCommandService
{
    public async Task<CharacterBatchResponse> CreateAsync(
        IReadOnlyCollection<CharacterRequest> requests,
        CancellationToken cancellationToken)
    {
        var validRequests = NormalizeRequests(requests);
        var names = validRequests.Select(request => CharactersMapper.NormalizeName(request.Name)).ToHashSet(StringComparer.Ordinal);
        var existing = await characterRepository.GetByIdsOrNamesAsync([], names, includeDeleted: true, cancellationToken);
        var existingNames = existing.Select(character => character.Name).ToHashSet(StringComparer.Ordinal);
        var now = dateTimeProvider.UtcNow;
        var created = new List<Domain.Entities.Characters.Character>();

        foreach (var request in validRequests)
        {
            var name = CharactersMapper.NormalizeName(request.Name);
            if (!existingNames.Add(name))
            {
                continue;
            }

            created.Add(mapper.MapNew(request, now));
        }

        await characterRepository.AddRangeAsync(created, cancellationToken);
        await characterRepository.SaveAsync(cancellationToken);

        logger.LogInformation("Characters create batch finished. Created {Created} Skipped {Skipped}", created.Count, requests.Count - created.Count);

        return new CharacterBatchResponse
        {
            TotalReceived = requests.Count,
            Created = created.Count,
            Skipped = requests.Count - created.Count,
            Items = created.Select(mapper.Map).ToList()
        };
    }

    public async Task<CharacterBatchResponse> UpdateAsync(
        IReadOnlyCollection<CharacterRequest> requests,
        CancellationToken cancellationToken)
    {
        var validRequests = NormalizeRequests(requests);
        var ids = validRequests.Where(request => request.Id.HasValue).Select(request => request.Id!.Value).ToHashSet();
        var names = validRequests.Select(request => CharactersMapper.NormalizeName(request.Name)).ToHashSet(StringComparer.Ordinal);
        var characters = await characterRepository.GetByIdsOrNamesAsync(ids, names, includeDeleted: false, cancellationToken);
        var now = dateTimeProvider.UtcNow;
        var updated = new List<Domain.Entities.Characters.Character>();

        foreach (var request in validRequests)
        {
            var name = CharactersMapper.NormalizeName(request.Name);
            var character = characters.FirstOrDefault(item =>
                (request.Id.HasValue && item.Id == request.Id.Value) ||
                string.Equals(item.Name, name, StringComparison.Ordinal));

            if (character is null)
            {
                continue;
            }

            mapper.Apply(character, request);
            character.UpdatedAt = now;
            updated.Add(character);
        }

        await characterRepository.SaveAsync(cancellationToken);

        logger.LogInformation("Characters update batch finished. Updated {Updated} NotFound {NotFound}", updated.Count, requests.Count - updated.Count);

        return new CharacterBatchResponse
        {
            TotalReceived = requests.Count,
            Updated = updated.Count,
            NotFound = requests.Count - updated.Count,
            Items = updated.Select(mapper.Map).ToList()
        };
    }

    public async Task<CharacterBatchResponse> DeleteAsync(
        IReadOnlyCollection<CharacterDeleteRequest> requests,
        CancellationToken cancellationToken)
    {
        var ids = requests.Where(request => request.Id.HasValue).Select(request => request.Id!.Value).ToHashSet();
        var names = requests
            .Where(request => !string.IsNullOrWhiteSpace(request.Name))
            .Select(request => CharactersMapper.NormalizeName(request.Name!))
            .ToHashSet(StringComparer.Ordinal);

        var characters = await characterRepository.GetByIdsOrNamesAsync(ids, names, includeDeleted: false, cancellationToken);
        var now = dateTimeProvider.UtcNow;
        var hardDelete = new List<Domain.Entities.Characters.Character>();
        var deleted = new List<Domain.Entities.Characters.Character>();

        foreach (var request in requests)
        {
            var name = string.IsNullOrWhiteSpace(request.Name) ? null : CharactersMapper.NormalizeName(request.Name);
            var character = characters.FirstOrDefault(item =>
                (request.Id.HasValue && item.Id == request.Id.Value) ||
                (name is not null && string.Equals(item.Name, name, StringComparison.Ordinal)));

            if (character is null || deleted.Contains(character))
            {
                continue;
            }

            if (request.DeleteMode == CharacterDeleteMode.HardDelete)
            {
                hardDelete.Add(character);
            }
            else
            {
                character.IsDeleted = true;
                character.UpdatedAt = now;
            }

            deleted.Add(character);
        }

        characterRepository.RemoveRange(hardDelete);
        await characterRepository.SaveAsync(cancellationToken);

        logger.LogInformation("Characters delete batch finished. Deleted {Deleted} NotFound {NotFound}", deleted.Count, requests.Count - deleted.Count);

        return new CharacterBatchResponse
        {
            TotalReceived = requests.Count,
            Deleted = deleted.Count,
            NotFound = requests.Count - deleted.Count,
            Items = deleted.Where(character => !hardDelete.Contains(character)).Select(mapper.Map).ToList()
        };
    }

    private static List<CharacterRequest> NormalizeRequests(IReadOnlyCollection<CharacterRequest> requests)
    {
        return requests
            .Where(request => !string.IsNullOrWhiteSpace(request.Name))
            .GroupBy(request => CharactersMapper.NormalizeName(request.Name), StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();
    }
}
