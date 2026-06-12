using StellarElysium.Domain.Enums.Localization;

namespace StellarElysium.Domain.Localization;

public static class SupportedLanguageExtensions
{
    public static string ToCultureCode(this SupportedLanguage language)
    {
        return language switch
        {
            SupportedLanguage.English => "en-US",
            SupportedLanguage.Spanish => "es-ES",
            _ => "pt-BR"
        };
    }

    public static bool TryParse(string? value, out SupportedLanguage language)
    {
        language = SupportedLanguage.Portuguese;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = value.Trim().Replace('_', '-').ToLowerInvariant();
        if (normalized.StartsWith("pt"))
        {
            language = SupportedLanguage.Portuguese;
            return true;
        }

        if (normalized.StartsWith("en"))
        {
            language = SupportedLanguage.English;
            return true;
        }

        if (normalized.StartsWith("es"))
        {
            language = SupportedLanguage.Spanish;
            return true;
        }

        if (normalized is "portuguese" or "portugues")
        {
            language = SupportedLanguage.Portuguese;
            return true;
        }

        if (normalized is "english")
        {
            language = SupportedLanguage.English;
            return true;
        }

        if (normalized is "spanish" or "espanol" or "español")
        {
            language = SupportedLanguage.Spanish;
            return true;
        }

        return false;
    }
}
