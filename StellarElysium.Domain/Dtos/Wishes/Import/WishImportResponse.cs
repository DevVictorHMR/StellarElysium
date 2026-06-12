namespace StellarElysium.Domain.Dtos.Wishes.Import;

public class WishImportResponse
{
    public Guid GenshinAccountId { get; set; }
    public int TotalReceived { get; set; }
    public int Imported { get; set; }
    public int Ignored { get; set; }
}
