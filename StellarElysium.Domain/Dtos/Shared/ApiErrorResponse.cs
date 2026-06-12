namespace StellarElysium.Domain.Dtos.Shared;

public class ApiErrorResponse : LocalizedResponse
{
    public string Code { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
