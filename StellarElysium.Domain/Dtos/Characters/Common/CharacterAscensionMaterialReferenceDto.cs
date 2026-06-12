namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterAscensionMaterialReferenceDto
{
    public Guid? AscensionMaterialId { get; set; }
    public string? MaterialSlug { get; set; }
    public int Quantity { get; set; }
    public int? RequiredLevel { get; set; }
    public int? AscensionPhase { get; set; }
    public string? Group { get; set; }
}
