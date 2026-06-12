namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterSoundDto
{
    public string SoundKey { get; set; } = string.Empty;
    public string? Category { get; set; }
    public Dictionary<string, string> AudioUrls { get; set; } = new();
    public int SortOrder { get; set; }
    public Dictionary<string, CharacterSoundLocalizationDto> Localizations { get; set; } = new();
}
