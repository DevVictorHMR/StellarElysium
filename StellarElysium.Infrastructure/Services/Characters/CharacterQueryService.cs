using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Characters;
using StellarElysium.Application.Interfaces.Services.Characters;
using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Infrastructure.Mappers.Characters;

namespace StellarElysium.Infrastructure.Services.Characters;

public sealed class CharacterQueryService(
    ICharacterRepository characterRepository,
    CharactersMapper mapper,
    ILogger<CharacterQueryService> logger) : ICharacterQueryService
{
    public async Task<PaginatedResponse<CharacterResponse>> ListAsync(
        CharacterQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Listing characters. Search {Search}", request.Search);

        var skip = request.Skip < 0 ? 0 : request.Skip;
        var take = request.Take <= 0 ? 20 : request.Take;
        if (take > 200)
        {
            take = 200;
        }

        var names = request.Names
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(CharactersMapper.NormalizeName)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var result = await characterRepository.ListAsync(
            names,
            request.Search,
            request.IncludeDeleted,
            skip,
            take,
            cancellationToken);

        return new PaginatedResponse<CharacterResponse>
        {
            Total = result.Total,
            Skip = skip,
            Take = take,
            Items = result.Items.Select(mapper.Map).ToList()
        };
    }
}
