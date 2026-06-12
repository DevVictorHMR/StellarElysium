using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;
using StellarElysium.Domain.Dtos.Shared;

namespace StellarElysium.Application.Interfaces.Services.Characters;

public interface ICharacterQueryService
{
    Task<PaginatedResponse<CharacterResponse>> ListAsync(
        CharacterQueryRequest request,
        CancellationToken cancellationToken);
}
