using Microsoft.AspNetCore.Mvc;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Dtos.Wishes.Query;
using StellarElysium.Domain.Dtos.Wishes.Import;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.WebApi.Controllers;

[ApiController]
[Route("api/wishes")]
public sealed class WishesController(
    IWishImportService importService,
    IWishHistoryUrlImportService urlImportService,
    IWishQueryService queryService,
    IWishPityService pityService,
    ILogger<WishesController> logger) : ControllerBase
{
    [HttpPost("import")]
    public async Task<ActionResult<WishImportResponse>> Import(
        [FromBody] WishImportRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Wish import request received. Total {Total}", request.Wishes.Count);
        var result = await importService.ImportAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("import-url")]
    public async Task<ActionResult<WishImportResponse>> ImportUrl(
        [FromBody] WishUrlImportRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Wish URL import request received. Uid {Uid}", request.Account.Uid);
        var result = await urlImportService.ImportByUrlAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<WishResponse>>> List(
        [FromQuery] Guid genshinAccountId,
        [FromQuery] int? gachaType,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? rankType,
        [FromQuery] string? name,
        [FromQuery] string? itemType,
        [FromQuery] WishSortField? sortBy,
        [FromQuery] SortDirection? direction,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        if (genshinAccountId == Guid.Empty)
        {
            return BadRequest("Invalid GenshinAccountId.");
        }

        var request = new WishQueryRequest
        {
            GenshinAccountId = genshinAccountId,
            GachaType = gachaType,
            StartDate = startDate,
            EndDate = endDate,
            RankType = rankType,
            Name = name,
            ItemType = itemType,
            SortBy = sortBy,
            Direction = direction ?? SortDirection.Desc,
            Skip = skip,
            Take = take
        };

        var wishes = await queryService.ListAsync(request, cancellationToken);
        return Ok(wishes);
    }

    [HttpGet("account/{genshinAccountId:guid}")]
    public async Task<ActionResult<PaginatedResponse<WishResponse>>> ListByAccount(
        Guid genshinAccountId,
        [FromQuery] int? gachaType,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? rankType,
        [FromQuery] string? name,
        [FromQuery] string? itemType,
        [FromQuery] WishSortField? sortBy,
        [FromQuery] SortDirection? direction,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        if (genshinAccountId == Guid.Empty)
        {
            return BadRequest("Invalid GenshinAccountId.");
        }

        var request = new WishQueryRequest
        {
            GenshinAccountId = genshinAccountId,
            GachaType = gachaType,
            StartDate = startDate,
            EndDate = endDate,
            RankType = rankType,
            Name = name,
            ItemType = itemType,
            SortBy = sortBy,
            Direction = direction ?? SortDirection.Desc,
            Skip = skip,
            Take = take
        };

        var wishes = await queryService.ListAsync(request, cancellationToken);
        return Ok(wishes);
    }

    [HttpGet("{genshinAccountId:guid}/pity")]
    public async Task<ActionResult<PitySummaryResponse>> GetPity(
        Guid genshinAccountId,
        CancellationToken cancellationToken)
    {
        if (genshinAccountId == Guid.Empty)
        {
            return BadRequest("Invalid GenshinAccountId.");
        }

        var result = await pityService.GetPityAsync(genshinAccountId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{genshinAccountId:guid}/pity-detail")]
    public async Task<ActionResult<PityDetailResponse>> GetPityDetail(
        Guid genshinAccountId,
        [FromQuery] int? gachaType,
        [FromQuery] int quantityItems = 5,
        CancellationToken cancellationToken = default)
    {
        if (genshinAccountId == Guid.Empty)
        {
            return BadRequest("Invalid GenshinAccountId.");
        }

        var request = new PityDetailRequest
        {
            GenshinAccountId = genshinAccountId,
            GachaType = gachaType,
            QuantityItems = quantityItems
        };

        var result = await pityService.GetPityDetailAsync(request, cancellationToken);
        return Ok(result);
    }
}
