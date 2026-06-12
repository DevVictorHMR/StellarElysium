using StellarElysium.Domain.Enums.Localization;

namespace StellarElysium.WebApi.Localization;

public interface IRequestLanguageProvider
{
    SupportedLanguage GetCurrentLanguage();
}
