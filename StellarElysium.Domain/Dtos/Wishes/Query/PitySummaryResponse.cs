using StellarElysium.Domain.Dtos.Shared;

namespace StellarElysium.Domain.Dtos.Wishes.Query;

public class PitySummaryResponse : LocalizedResponse
{
    public Guid GenshinAccountId { get; set; }
    public List<PityBannerResponse> Banners { get; set; } = new();
}
