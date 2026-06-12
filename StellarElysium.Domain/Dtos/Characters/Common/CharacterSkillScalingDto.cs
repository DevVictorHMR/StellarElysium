namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterSkillScalingDto
{
    public string LabelKey { get; set; } = string.Empty;
    public Dictionary<string, CharacterSkillScalingLocalizationDto> Localizations { get; set; } = new();
    public List<CharacterSkillScalingValueDto> ValuesByLevel { get; set; } = new();
}
