using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Dtos.Characters.Requests;

public record CharacterDeleteRequest
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public CharacterDeleteMode DeleteMode { get; set; } = CharacterDeleteMode.SoftDelete;
}
