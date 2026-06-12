using System.Globalization;
using StellarElysium.Application.Interfaces.Localization;
using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Localization;

namespace StellarElysium.Infrastructure.Services.Localization;

public sealed class ApiMessageLocalizer : IApiMessageLocalizer
{
    private static readonly IReadOnlyDictionary<SupportedLanguage, IReadOnlyDictionary<ApiMessageKey, string>> Messages =
        new Dictionary<SupportedLanguage, IReadOnlyDictionary<ApiMessageKey, string>>
        {
            [SupportedLanguage.Portuguese] = new Dictionary<ApiMessageKey, string>
            {
                [ApiMessageKey.InvalidRequest] = "A requisição enviada é inválida.",
                [ApiMessageKey.InvalidField] = "Campo inválido.",
                [ApiMessageKey.UnexpectedError] = "Ocorreu um erro inesperado. Tente novamente em instantes.",
                [ApiMessageKey.InvalidGenshinAccountId] = "O identificador da conta Genshin é inválido.",
                [ApiMessageKey.InvalidHistoryUrl] = "A URL do histórico de desejos é inválida.",
                [ApiMessageKey.HistoryUrlMissingAuthkey] = "A URL do histórico de desejos não possui authkey.",
                [ApiMessageKey.WishImportSucceeded] = "Histórico de desejos importado com sucesso.",
                [ApiMessageKey.WishUrlImportSucceeded] = "Histórico de desejos importado pela URL com sucesso.",
                [ApiMessageKey.WishesListedSucceeded] = "Histórico de desejos consultado com sucesso.",
                [ApiMessageKey.PitySummaryRetrieved] = "Resumo de pity consultado com sucesso.",
                [ApiMessageKey.PityDetailRetrieved] = "Detalhe de pity consultado com sucesso."
            },
            [SupportedLanguage.English] = new Dictionary<ApiMessageKey, string>
            {
                [ApiMessageKey.InvalidRequest] = "The submitted request is invalid.",
                [ApiMessageKey.InvalidField] = "Invalid field.",
                [ApiMessageKey.UnexpectedError] = "An unexpected error occurred. Please try again shortly.",
                [ApiMessageKey.InvalidGenshinAccountId] = "The Genshin account identifier is invalid.",
                [ApiMessageKey.InvalidHistoryUrl] = "The wish history URL is invalid.",
                [ApiMessageKey.HistoryUrlMissingAuthkey] = "The wish history URL is missing authkey.",
                [ApiMessageKey.WishImportSucceeded] = "Wish history imported successfully.",
                [ApiMessageKey.WishUrlImportSucceeded] = "Wish history imported from URL successfully.",
                [ApiMessageKey.WishesListedSucceeded] = "Wish history retrieved successfully.",
                [ApiMessageKey.PitySummaryRetrieved] = "Pity summary retrieved successfully.",
                [ApiMessageKey.PityDetailRetrieved] = "Pity detail retrieved successfully."
            },
            [SupportedLanguage.Spanish] = new Dictionary<ApiMessageKey, string>
            {
                [ApiMessageKey.InvalidRequest] = "La solicitud enviada no es válida.",
                [ApiMessageKey.InvalidField] = "Campo no válido.",
                [ApiMessageKey.UnexpectedError] = "Se produjo un error inesperado. Inténtalo de nuevo en unos instantes.",
                [ApiMessageKey.InvalidGenshinAccountId] = "El identificador de la cuenta de Genshin no es válido.",
                [ApiMessageKey.InvalidHistoryUrl] = "La URL del historial de deseos no es válida.",
                [ApiMessageKey.HistoryUrlMissingAuthkey] = "La URL del historial de deseos no contiene authkey.",
                [ApiMessageKey.WishImportSucceeded] = "Historial de deseos importado correctamente.",
                [ApiMessageKey.WishUrlImportSucceeded] = "Historial de deseos importado desde la URL correctamente.",
                [ApiMessageKey.WishesListedSucceeded] = "Historial de deseos consultado correctamente.",
                [ApiMessageKey.PitySummaryRetrieved] = "Resumen de pity consultado correctamente.",
                [ApiMessageKey.PityDetailRetrieved] = "Detalle de pity consultado correctamente."
            }
        };

    public string Get(ApiMessageKey key, SupportedLanguage language, params object[] args)
    {
        var template = GetTemplate(key, language);
        if (args.Length == 0)
        {
            return template;
        }

        return string.Format(CultureInfo.GetCultureInfo(language.ToCultureCode()), template, args);
    }

    private static string GetTemplate(ApiMessageKey key, SupportedLanguage language)
    {
        if (Messages.TryGetValue(language, out var translations) && translations.TryGetValue(key, out var message))
        {
            return message;
        }

        return Messages[SupportedLanguage.Portuguese][key];
    }
}
