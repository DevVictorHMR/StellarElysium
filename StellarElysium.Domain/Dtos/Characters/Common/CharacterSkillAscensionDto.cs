namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterSkillAscensionDto
{
    public string SkillKey { get; set; } = string.Empty;
    public int TargetLevel { get; set; }
    public int MoraCost { get; set; }
    public List<CharacterAscensionMaterialReferenceDto> Materials { get; set; } = new();
}
