namespace StellarElysium.Domain.Dtos.Characters.Requests;

public record CharacterQueryRequest
{
    public List<string> Names { get; set; } = new();
    public string? Search { get; set; }
    public bool IncludeDeleted { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 100;
}
