using StellarElysium.Domain.Enums.Localization;

namespace StellarElysium.Application.Interfaces.Localization;

public interface IApiMessageLocalizer
{
    string Get(ApiMessageKey key, SupportedLanguage language, params object[] args);
}
