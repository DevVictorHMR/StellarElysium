namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterSkillLocalizationDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
