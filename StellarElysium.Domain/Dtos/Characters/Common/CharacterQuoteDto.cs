namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterQuoteDto
{
    public string QuoteKey { get; set; } = string.Empty;
    public string? SoundKey { get; set; }
    public string? UnlockCondition { get; set; }
    public int SortOrder { get; set; }
    public Dictionary<string, CharacterTextLocalizationDto> Localizations { get; set; } = new();
}
