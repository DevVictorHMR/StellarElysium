using StellarElysium.Domain.Dtos.Shared;

namespace StellarElysium.Domain.Dtos.Characters.Responses;

public class CharacterBatchResponse : LocalizedResponse
{
    public int TotalReceived { get; set; }
    public int Created { get; set; }
    public int Updated { get; set; }
    public int Deleted { get; set; }
    public int Skipped { get; set; }
    public int NotFound { get; set; }
    public List<CharacterResponse> Items { get; set; } = new();
}
