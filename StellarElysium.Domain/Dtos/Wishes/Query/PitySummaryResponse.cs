namespace StellarElysium.Domain.Dtos.Wishes.Query;

public class PitySummaryResponse
{
    public Guid GenshinAccountId { get; set; }
    public List<PityBannerResponse> Banners { get; set; } = new();
}
