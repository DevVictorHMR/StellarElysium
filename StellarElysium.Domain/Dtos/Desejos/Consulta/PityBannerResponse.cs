namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class PityBannerResponse
{
    public int GachaType { get; set; }
    public int Total { get; set; }
    public int PityCinco { get; set; }
    public int PityQuatro { get; set; }
    public DateTime? UltimoCinco { get; set; }
    public DateTime? UltimoQuatro { get; set; }
    public string? UltimoCincoNome { get; set; }
    public string? UltimoQuatroNome { get; set; }
}
