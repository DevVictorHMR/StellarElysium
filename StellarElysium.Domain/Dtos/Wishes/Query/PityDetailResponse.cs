namespace StellarElysium.Domain.Dtos.Wishes.Query;

public class PityDetailResponse
{
    public Guid GenshinAccountId { get; set; }
    public List<PityBannerDetailResponse> Banners { get; set; } = new();
}

public class PityBannerDetailResponse
{
    public int GachaType { get; set; }
    public int FiveStarPity { get; set; }
    public int FourStarPity { get; set; }
    public List<PityItemResponse> LastFiveStars { get; set; } = new();
    public List<PityItemResponse> LastFourStars { get; set; } = new();
}
