using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.Domain.Dtos.Wishes.Accounts;

public record GenshinAccountResponse
{
    public Guid Id { get; set; }
    public string Uid { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public GenshinServer Server { get; set; }
}
