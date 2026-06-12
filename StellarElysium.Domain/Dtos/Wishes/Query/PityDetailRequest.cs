using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Dtos.Wishes.Query;

public record PityDetailRequest
{
    [Required]
    public Guid GenshinAccountId { get; set; }

    public int? GachaType { get; set; }

    public int QuantityItems { get; set; } = 5;
}
