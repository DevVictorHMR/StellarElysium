namespace StellarElysium.Domain.Dtos.Desejos.Consulta;

public class PityResumoResponse
{
    public Guid ContaGenshinId { get; set; }
    public List<PityBannerResponse> Banners { get; set; } = new();
}
