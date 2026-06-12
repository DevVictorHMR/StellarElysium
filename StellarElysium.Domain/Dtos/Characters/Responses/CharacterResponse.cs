using StellarElysium.Domain.Dtos.Characters.Common;

namespace StellarElysium.Domain.Dtos.Characters.Responses;

public record CharacterResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? GameCode { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CharacterProfileDto Profile { get; set; } = new();
    public List<CharacterStatDto> Stats { get; set; } = new();
    public List<CharacterSkillDto> Skills { get; set; } = new();
    public List<CharacterSkillAscensionDto> SkillAscension { get; set; } = new();
    public List<CharacterRelatedItemDto> RelatedItems { get; set; } = new();
    public List<CharacterGalleryItemDto> Gallery { get; set; } = new();
    public List<CharacterSoundDto> Sounds { get; set; } = new();
    public List<CharacterQuoteDto> Quotes { get; set; } = new();
    public List<CharacterStoryDto> Stories { get; set; } = new();
}
