using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterSkillDto
{
    public string SkillKey { get; set; } = string.Empty;
    public CharacterSkillType Type { get; set; } = CharacterSkillType.Other;
    public int SortOrder { get; set; }
    public int? MaxLevel { get; set; }
    public string? IconUrl { get; set; }
    public Dictionary<string, CharacterSkillLocalizationDto> Localizations { get; set; } = new();
    public List<CharacterSkillScalingDto> Scalings { get; set; } = new();
}
