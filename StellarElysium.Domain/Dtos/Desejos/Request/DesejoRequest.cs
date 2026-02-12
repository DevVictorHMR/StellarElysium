using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Dtos.Desejos.Request;

public class DesejoRequest
{
    [Required]
    public Guid ContaGenshinId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PullId { get; set; } = string.Empty;

    [Required]
    public int GachaType { get; set; }

    [MaxLength(30)]
    public string? ItemId { get; set; }

    [Required]
    [MaxLength(100)]
    public string NomeItem { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string TipoItem { get; set; } = string.Empty;

    [Required]
    public int RankType { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Required]
    public DateTime DataHoraDesejo { get; set; }
}
