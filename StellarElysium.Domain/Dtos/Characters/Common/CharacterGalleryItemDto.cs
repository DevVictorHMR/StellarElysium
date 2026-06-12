using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterGalleryItemDto
{
    public string AssetKey { get; set; } = string.Empty;
    public CharacterGalleryItemType Type { get; set; } = CharacterGalleryItemType.Other;
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int SortOrder { get; set; }
    public Dictionary<string, CharacterMediaLocalizationDto> Localizations { get; set; } = new();
}
