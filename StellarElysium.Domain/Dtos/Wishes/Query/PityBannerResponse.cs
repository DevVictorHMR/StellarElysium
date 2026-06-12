namespace StellarElysium.Domain.Dtos.Wishes.Query;

public record PityBannerResponse
{
    public int GachaType { get; set; }
    public int Total { get; set; }
    public int FiveStarPity { get; set; }
    public int FourStarPity { get; set; }
    public DateTime? LastFiveStar { get; set; }
    public DateTime? LastFourStar { get; set; }
    public string? LastFiveStarName { get; set; }
    public string? LastFourStarName { get; set; }
}
