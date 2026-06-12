using StellarElysium.Domain.Dtos.Shared;

namespace StellarElysium.Domain.Dtos.Wishes.Import;

public class WishImportResponse : LocalizedResponse
{
    public Guid GenshinAccountId { get; set; }
    public int TotalReceived { get; set; }
    public int Imported { get; set; }
    public int Ignored { get; set; }
}
