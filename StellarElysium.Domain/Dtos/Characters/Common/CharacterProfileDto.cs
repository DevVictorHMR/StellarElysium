using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterProfileDto
{
    public int Rarity { get; set; }
    public CharacterElement Element { get; set; } = CharacterElement.Unknown;
    public CharacterWeaponType WeaponType { get; set; } = CharacterWeaponType.Unknown;
    public string? Region { get; set; }
    public string? Association { get; set; }
    public CharacterBirthdayDto? Birthday { get; set; }
    public string? Constellation { get; set; }
    public CharacterVoiceActorsDto VoiceActors { get; set; } = new();
    public Dictionary<string, CharacterProfileLocalizationDto> Localizations { get; set; } = new();
    public List<CharacterAscensionMaterialReferenceDto> CharacterAscensionMaterials { get; set; } = new();
    public List<CharacterAscensionMaterialReferenceDto> SkillAscensionMaterials { get; set; } = new();
}
