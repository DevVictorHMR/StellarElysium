using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Wishes.Import;

namespace StellarElysium.Infrastructure.Services.Wishes;

public sealed class WishHistoryUrlImportService(
    IWishHistoryService historyService,
    IWishImportService importService,
    ILogger<WishHistoryUrlImportService> logger) : IWishHistoryUrlImportService
{
    public async Task<WishImportResponse> ImportByUrlAsync(
        WishUrlImportRequest request,
        CancellationToken cancellationToken)
    {
        var gachaTypes = request.GachaTypes ?? [];
        logger.LogInformation("Wish URL import started. Uid {Uid} Types {Types}",
            request.Account.Uid,
            gachaTypes.Count == 0 ? "default" : string.Join(",", gachaTypes));

        var wishes = await historyService.FetchAsync(request.Url, gachaTypes, cancellationToken);
        var importRequest = new WishImportRequest
        {
            Account = request.Account,
            Wishes = wishes.ToList()
        };

        var result = await importService.ImportAsync(importRequest, cancellationToken);
        logger.LogInformation("Wish URL import finished. Imported {Imported} Ignored {Ignored}",
            result.Imported,
            result.Ignored);

        return result;
    }
}
