namespace StellarElysium.Domain.Dtos.Shared;

public record LocalizedResponse
{
    public string Message { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}
