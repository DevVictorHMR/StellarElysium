namespace StellarElysium.Domain.Dtos.Shared;

public class PaginatedResponse<T> : LocalizedResponse
{
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public List<T> Items { get; set; } = new();
}
