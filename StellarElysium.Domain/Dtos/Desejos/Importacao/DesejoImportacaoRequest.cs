using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StellarElysium.Domain.Dtos.Desejos.Importacao;

public class DesejoImportacaoRequest
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
    public string Quantidade { get; set; } = "1";

    [Required]
    [MaxLength(19)]
    [JsonPropertyName("time")]
    public string DataHora { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [JsonPropertyName("name")]
    public string NomeItem { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [JsonPropertyName("item_type")]
    public string TipoItem { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    [JsonPropertyName("rank_type")]
    public string RankType { get; set; } = string.Empty;

    [MaxLength(10)]
    [JsonPropertyName("lang")]
    public string? Idioma { get; set; }
}
