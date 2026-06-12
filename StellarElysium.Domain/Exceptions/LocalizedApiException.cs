using StellarElysium.Domain.Enums.Localization;

namespace StellarElysium.Domain.Exceptions;

public sealed class LocalizedApiException : Exception
{
    public LocalizedApiException(ApiMessageKey messageKey, int statusCode = 400)
    {
        MessageKey = messageKey;
        StatusCode = statusCode;
    }

    public ApiMessageKey MessageKey { get; }
    public int StatusCode { get; }
}
