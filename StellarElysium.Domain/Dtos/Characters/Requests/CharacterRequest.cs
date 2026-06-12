using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Dtos.Characters.Common;

namespace StellarElysium.Domain.Dtos.Characters.Requests;

public class CharacterRequest
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? GameCode { get; set; }

    [Required]
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
