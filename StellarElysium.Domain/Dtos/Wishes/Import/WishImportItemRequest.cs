using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StellarElysium.Domain.Dtos.Wishes.Import;

public class WishImportItemRequest
{
    [Required]
    [MaxLength(50)]
    [JsonPropertyName("id")]
    public string PullId { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [JsonPropertyName("gacha_type")]
    public string GachaType { get; set; } = string.Empty;

    [MaxLength(30)]
    [JsonPropertyName("item_id")]
    public string? ItemId { get; set; }

    [Required]
    [MaxLength(3)]
    [JsonPropertyName("count")]
    public string Quantity { get; set; } = "1";

    [Required]
    [MaxLength(19)]
    [JsonPropertyName("time")]
    public string WishTime { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [JsonPropertyName("name")]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [JsonPropertyName("item_type")]
    public string ItemType { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    [JsonPropertyName("rank_type")]
    public string RankType { get; set; } = string.Empty;

    [MaxLength(10)]
    [JsonPropertyName("lang")]
    public string? Language { get; set; }
}
