using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums;

namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class DesejoConsultaRequest
{
    [Required]
    public Guid ContaGenshinId { get; set; }

    public int? GachaType { get; set; }

    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public int? RankType { get; set; }

    [MaxLength(100)]
    public string? Nome { get; set; }

    [MaxLength(30)]
    public string? TipoItem { get; set; }

    public DesejoOrdenacaoCampo? OrdenarPor { get; set; }
    public OrdenacaoDirecao Direcao { get; set; } = OrdenacaoDirecao.Desc;

    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 100;
}
