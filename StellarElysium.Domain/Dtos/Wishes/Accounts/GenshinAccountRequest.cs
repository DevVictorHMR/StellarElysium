using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.Domain.Dtos.Wishes.Accounts;

public record GenshinAccountRequest
{
    [Required]
    [MaxLength(20)]
    public string Uid { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Nickname { get; set; }

    [Required]
    public GenshinServer Server { get; set; }
}
