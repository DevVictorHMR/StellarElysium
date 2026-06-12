using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Dtos.Wishes.Accounts;

namespace StellarElysium.Domain.Dtos.Wishes.Import;

public class WishUrlImportRequest
{
    [Required]
    public GenshinAccountRequest Account { get; set; } = new();

    [Required]
    [MaxLength(2000)]
    public string Url { get; set; } = string.Empty;

    public List<int>? GachaTypes { get; set; }
}
