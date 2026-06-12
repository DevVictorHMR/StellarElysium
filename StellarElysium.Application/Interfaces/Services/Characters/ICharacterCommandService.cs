using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;

namespace StellarElysium.Application.Interfaces.Services.Characters;

public interface ICharacterCommandService
{
    Task<CharacterBatchResponse> CreateAsync(
        IReadOnlyCollection<CharacterRequest> requests,
        CancellationToken cancellationToken);

    Task<CharacterBatchResponse> UpdateAsync(
        IReadOnlyCollection<CharacterRequest> requests,
        CancellationToken cancellationToken);

    Task<CharacterBatchResponse> DeleteAsync(
        IReadOnlyCollection<CharacterDeleteRequest> requests,
        CancellationToken cancellationToken);
}
