using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class PityDetalheRequest
{
    [Required]
    public Guid ContaGenshinId { get; set; }

    public int? GachaType { get; set; }

    public int QuantidadeItens { get; set; } = 5;
}
