using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterRelatedItemDto
{
    public string ItemSlug { get; set; } = string.Empty;
    public CharacterRelatedItemType Type { get; set; } = CharacterRelatedItemType.Other;
    public int? Rarity { get; set; }
    public string? IconUrl { get; set; }
    public Dictionary<string, CharacterRelatedItemLocalizationDto> Localizations { get; set; } = new();
}
