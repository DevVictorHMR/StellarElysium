using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Dtos.ContaGenshin;

namespace StellarElysium.Domain.Dtos.Desejos.Importacao;

public class ImportacaoDesejosUrlRequest
{
    [Required]
    public ContaGenshinRequest Conta { get; set; } = new();

    [Required]
    [MaxLength(2000)]
    public string Url { get; set; } = string.Empty;

    public List<int>? GachaTypes { get; set; }
}
