namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class PityDetalheResponse
{
    public Guid ContaGenshinId { get; set; }
    public List<PityBannerDetalheResponse> Banners { get; set; } = new();
}

public class PityBannerDetalheResponse
{
    public int GachaType { get; set; }
    public int PityCinco { get; set; }
    public int PityQuatro { get; set; }
    public List<PityItemResponse> UltimosCinco { get; set; } = new();
    public List<PityItemResponse> UltimosQuatro { get; set; } = new();
}
