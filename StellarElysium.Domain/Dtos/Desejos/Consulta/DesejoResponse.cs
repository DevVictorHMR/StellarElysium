namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class DesejoResponse
{
    public Guid Id { get; set; }
    public Guid ContaGenshinId { get; set; }
    public string PullId { get; set; } = string.Empty;
    public int GachaType { get; set; }
    public string? ItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public string TipoItem { get; set; } = string.Empty;
    public int RankType { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataHoraDesejo { get; set; }
}
