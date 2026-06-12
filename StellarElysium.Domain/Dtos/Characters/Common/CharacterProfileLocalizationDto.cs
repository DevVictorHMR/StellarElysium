namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterProfileLocalizationDto
{
    public string Name { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Occupation { get; set; }
    public string? Description { get; set; }
}
