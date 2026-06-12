using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Dtos.Wishes.Requests;

public record WishRequest
{
    [Required]
    public Guid GenshinAccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PullId { get; set; } = string.Empty;

    [Required]
    public int GachaType { get; set; }

    [MaxLength(30)]
    public string? ItemId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string ItemType { get; set; } = string.Empty;

    [Required]
    public int RankType { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public DateTime WishTime { get; set; }
}
