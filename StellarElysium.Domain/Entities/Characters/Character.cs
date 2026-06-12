using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums.Characters;

namespace StellarElysium.Domain.Entities.Characters;

public class Character : BaseEntity
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? GameCode { get; set; }

    [Required]
    public int Rarity { get; set; }

    [Required]
    public CharacterElement Element { get; set; } = CharacterElement.Unknown;

    [Required]
    public CharacterWeaponType WeaponType { get; set; } = CharacterWeaponType.Unknown;

    [MaxLength(50)]
    public string? Region { get; set; }

    [MaxLength(80)]
    public string? Association { get; set; }

    public int? BirthdayDay { get; set; }
    public int? BirthdayMonth { get; set; }

    [MaxLength(100)]
    public string? Constellation { get; set; }

    [MaxLength(100)]
    public string? ChineseVoiceActor { get; set; }

    [MaxLength(100)]
    public string? JapaneseVoiceActor { get; set; }

    [MaxLength(100)]
    public string? EnglishVoiceActor { get; set; }

    [MaxLength(100)]
    public string? KoreanVoiceActor { get; set; }

    [Required]
    public string ProfileLocalizationsJson { get; set; } = "{}";

    [Required]
    public string CharacterAscensionMaterialsJson { get; set; } = "[]";

    [Required]
    public string SkillAscensionMaterialsJson { get; set; } = "[]";

    [Required]
    public string StatsJson { get; set; } = "[]";

    [Required]
    public string SkillsJson { get; set; } = "[]";

    [Required]
    public string SkillAscensionJson { get; set; } = "[]";

    [Required]
    public string RelatedItemsJson { get; set; } = "[]";

    [Required]
    public string GalleryJson { get; set; } = "[]";

    [Required]
    public string SoundsJson { get; set; } = "[]";

    [Required]
    public string QuotesJson { get; set; } = "[]";

    [Required]
    public string StoriesJson { get; set; } = "[]";

    [Required]
    public bool IsDeleted { get; set; }
}
