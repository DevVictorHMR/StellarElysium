using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Localization;

namespace StellarElysium.WebApi.Localization;

public sealed class RequestLanguageProvider(IHttpContextAccessor httpContextAccessor) : IRequestLanguageProvider
{
    public SupportedLanguage GetCurrentLanguage()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null)
        {
            return SupportedLanguage.Portuguese;
        }

        if (SupportedLanguageExtensions.TryParse(request.Query["language"].FirstOrDefault(), out var queryLanguage))
        {
            return queryLanguage;
        }

        if (SupportedLanguageExtensions.TryParse(request.Query["lang"].FirstOrDefault(), out var queryLang))
        {
            return queryLang;
        }

        if (SupportedLanguageExtensions.TryParse(request.Headers["X-Language"].FirstOrDefault(), out var headerLanguage))
        {
            return headerLanguage;
        }

        var acceptLanguage = request.Headers.AcceptLanguage.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(acceptLanguage))
        {
            var preferred = acceptLanguage.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Split(';', StringSplitOptions.RemoveEmptyEntries)[0])
                .FirstOrDefault();

            if (SupportedLanguageExtensions.TryParse(preferred, out var acceptedLanguage))
            {
                return acceptedLanguage;
            }
        }

        return SupportedLanguage.Portuguese;
    }
}
