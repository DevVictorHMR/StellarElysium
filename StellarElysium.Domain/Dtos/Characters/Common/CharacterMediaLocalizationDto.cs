namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterMediaLocalizationDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? AltText { get; set; }
}
