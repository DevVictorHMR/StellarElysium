namespace StellarElysium.Domain.Dtos.Characters.Common;

public class CharacterStoryDto
{
    public string StoryKey { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string? UnlockCondition { get; set; }
    public Dictionary<string, CharacterTextLocalizationDto> Localizations { get; set; } = new();
}
