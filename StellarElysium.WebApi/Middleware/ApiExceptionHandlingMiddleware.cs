using StellarElysium.Application.Interfaces.Localization;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Exceptions;
using StellarElysium.Domain.Localization;
using StellarElysium.WebApi.Localization;

namespace StellarElysium.WebApi.Middleware;

public sealed class ApiExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ApiExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(
        HttpContext context,
        IApiMessageLocalizer localizer,
        IRequestLanguageProvider languageProvider)
    {
        try
        {
            await next(context);
        }
        catch (LocalizedApiException exception)
        {
            logger.LogWarning(exception, "Localized API exception. Key {MessageKey}", exception.MessageKey);
            await WriteErrorAsync(context, localizer, languageProvider, exception.MessageKey, exception.StatusCode);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception.");
            await WriteErrorAsync(context, localizer, languageProvider, ApiMessageKey.UnexpectedError, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        IApiMessageLocalizer localizer,
        IRequestLanguageProvider languageProvider,
        ApiMessageKey key,
        int statusCode)
    {
        var language = languageProvider.GetCurrentLanguage();

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse
        {
            Code = key.ToString(),
            Message = localizer.Get(key, language),
            Language = language.ToCultureCode()
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
