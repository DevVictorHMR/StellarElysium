namespace StellarElysium.Domain.Dtos.Wishes.Query;

public class PityItemResponse
{
    public string ItemName { get; set; } = string.Empty;
    public int RankType { get; set; }
    public DateTime WishTime { get; set; }
}
