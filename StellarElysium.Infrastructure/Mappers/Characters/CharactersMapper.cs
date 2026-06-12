using System.Text.Json;
using System.Text.Json.Serialization;
using StellarElysium.Domain.Dtos.Characters.Common;
using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;
using StellarElysium.Domain.Entities.Characters;

namespace StellarElysium.Infrastructure.Mappers.Characters;

public sealed class CharactersMapper
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public Character MapNew(CharacterRequest request, DateTime createdAt)
    {
        var character = new Character
        {
            Id = Guid.NewGuid(),
            CreatedAt = createdAt
        };

        Apply(character, request);
        return character;
    }

    public void Apply(Character character, CharacterRequest request)
    {
        character.Name = NormalizeName(request.Name);
        character.GameCode = NormalizeOptional(request.GameCode);

        var profile = request.Profile ?? new CharacterProfileDto();
        character.Rarity = profile.Rarity;
        character.Element = profile.Element;
        character.WeaponType = profile.WeaponType;
        character.Region = NormalizeOptional(profile.Region);
        character.Association = NormalizeOptional(profile.Association);
        character.BirthdayDay = profile.Birthday?.Day;
        character.BirthdayMonth = profile.Birthday?.Month;
        character.Constellation = NormalizeOptional(profile.Constellation);
        character.ChineseVoiceActor = NormalizeOptional(profile.VoiceActors.Chinese);
        character.JapaneseVoiceActor = NormalizeOptional(profile.VoiceActors.Japanese);
        character.EnglishVoiceActor = NormalizeOptional(profile.VoiceActors.English);
        character.KoreanVoiceActor = NormalizeOptional(profile.VoiceActors.Korean);
        character.ProfileLocalizationsJson = Serialize(profile.Localizations);
        character.CharacterAscensionMaterialsJson = Serialize(profile.CharacterAscensionMaterials);
        character.SkillAscensionMaterialsJson = Serialize(profile.SkillAscensionMaterials);
        character.StatsJson = Serialize(request.Stats);
        character.SkillsJson = Serialize(request.Skills);
        character.SkillAscensionJson = Serialize(request.SkillAscension);
        character.RelatedItemsJson = Serialize(request.RelatedItems);
        character.GalleryJson = Serialize(request.Gallery);
        character.SoundsJson = Serialize(request.Sounds);
        character.QuotesJson = Serialize(request.Quotes);
        character.StoriesJson = Serialize(request.Stories);
    }

    public CharacterResponse Map(Character character)
    {
        return new CharacterResponse
        {
            Id = character.Id,
            Name = character.Name,
            GameCode = character.GameCode,
            IsDeleted = character.IsDeleted,
            CreatedAt = character.CreatedAt,
            UpdatedAt = character.UpdatedAt,
            Profile = new CharacterProfileDto
            {
                Rarity = character.Rarity,
                Element = character.Element,
                WeaponType = character.WeaponType,
                Region = character.Region,
                Association = character.Association,
                Birthday = new CharacterBirthdayDto
                {
                    Day = character.BirthdayDay,
                    Month = character.BirthdayMonth
                },
                Constellation = character.Constellation,
                VoiceActors = new CharacterVoiceActorsDto
                {
                    Chinese = character.ChineseVoiceActor,
                    Japanese = character.JapaneseVoiceActor,
                    English = character.EnglishVoiceActor,
                    Korean = character.KoreanVoiceActor
                },
                Localizations = Deserialize(character.ProfileLocalizationsJson, new Dictionary<string, CharacterProfileLocalizationDto>()),
                CharacterAscensionMaterials = Deserialize(character.CharacterAscensionMaterialsJson, new List<CharacterAscensionMaterialReferenceDto>()),
                SkillAscensionMaterials = Deserialize(character.SkillAscensionMaterialsJson, new List<CharacterAscensionMaterialReferenceDto>())
            },
            Stats = Deserialize(character.StatsJson, new List<CharacterStatDto>()),
            Skills = Deserialize(character.SkillsJson, new List<CharacterSkillDto>()),
            SkillAscension = Deserialize(character.SkillAscensionJson, new List<CharacterSkillAscensionDto>()),
            RelatedItems = Deserialize(character.RelatedItemsJson, new List<CharacterRelatedItemDto>()),
            Gallery = Deserialize(character.GalleryJson, new List<CharacterGalleryItemDto>()),
            Sounds = Deserialize(character.SoundsJson, new List<CharacterSoundDto>()),
            Quotes = Deserialize(character.QuotesJson, new List<CharacterQuoteDto>()),
            Stories = Deserialize(character.StoriesJson, new List<CharacterStoryDto>())
        };
    }

    public static string NormalizeName(string name)
    {
        return name.Trim().ToLowerInvariant();
    }

    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, JsonOptions);
    }

    private static T Deserialize<T>(string? json, T fallback)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return fallback;
        }

        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? fallback;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
