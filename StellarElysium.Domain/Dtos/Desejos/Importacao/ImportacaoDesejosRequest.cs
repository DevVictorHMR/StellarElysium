using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Dtos.ContaGenshin;

namespace StellarElysium.Domain.Dtos.Desejos.Importacao;

public class ImportacaoDesejosRequest
{
    [Required]
    public ContaGenshinRequest Conta { get; set; } = new();

    [Required]
    [MinLength(1)]
    public List<DesejoImportacaoRequest> Desejos { get; set; } = new();
}
