namespace StellarElysium.Domain.Dtos.Wishes.Query;

public record WishResponse
{
    public Guid Id { get; set; }
    public Guid GenshinAccountId { get; set; }
    public string PullId { get; set; } = string.Empty;
    public int GachaType { get; set; }
    public string? ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public int RankType { get; set; }
    public int Quantity { get; set; }
    public DateTime WishTime { get; set; }
}
