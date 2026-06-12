using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Dtos.Wishes.Accounts;

namespace StellarElysium.Domain.Dtos.Wishes.Import;

public record WishImportRequest
{
    [Required]
    public GenshinAccountRequest Account { get; set; } = new();

    [Required]
    [MinLength(1)]
    public List<WishImportItemRequest> Wishes { get; set; } = new();
}
