namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterSkillScalingValueDto
{
    public int Level { get; set; }
    public string Value { get; set; } = string.Empty;
}
