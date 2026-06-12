using Microsoft.AspNetCore.Mvc;
using StellarElysium.Application.Interfaces.Localization;
using StellarElysium.Application.Interfaces.Services.Characters;
using StellarElysium.Domain.Dtos.Characters.Requests;
using StellarElysium.Domain.Dtos.Characters.Responses;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Localization;
using StellarElysium.WebApi.Localization;

namespace StellarElysium.WebApi.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharactersController(
    ICharacterCommandService commandService,
    ICharacterQueryService queryService,
    IApiMessageLocalizer messageLocalizer,
    IRequestLanguageProvider languageProvider,
    ILogger<CharactersController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<CharacterResponse>>> List(
        [FromQuery] string[]? names,
        [FromQuery] string? search,
        [FromQuery] bool includeDeleted = false,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        var request = new CharacterQueryRequest
        {
            Names = names?.ToList() ?? new List<string>(),
            Search = search,
            IncludeDeleted = includeDeleted,
            Skip = skip,
            Take = take
        };

        var characters = await queryService.ListAsync(request, cancellationToken);
        return Ok(Localize(characters, ApiMessageKey.CharactersRetrieved));
    }

    [HttpPost]
    public async Task<ActionResult<CharacterBatchResponse>> Create(
        [FromBody] List<CharacterRequest> requests,
        CancellationToken cancellationToken)
    {
        if (requests.Count == 0)
        {
            return BadRequestMessage(ApiMessageKey.InvalidRequest);
        }

        logger.LogInformation("Character create batch received. Total {Total}", requests.Count);
        var result = await commandService.CreateAsync(requests, cancellationToken);
        return Ok(Localize(result, ApiMessageKey.CharactersCreated));
    }

    [HttpPut]
    public async Task<ActionResult<CharacterBatchResponse>> Update(
        [FromBody] List<CharacterRequest> requests,
        CancellationToken cancellationToken)
    {
        if (requests.Count == 0)
        {
            return BadRequestMessage(ApiMessageKey.InvalidRequest);
        }

        logger.LogInformation("Character update batch received. Total {Total}", requests.Count);
        var result = await commandService.UpdateAsync(requests, cancellationToken);
        return Ok(Localize(result, ApiMessageKey.CharactersUpdated));
    }

    [HttpDelete]
    public async Task<ActionResult<CharacterBatchResponse>> Delete(
        [FromBody] List<CharacterDeleteRequest> requests,
        CancellationToken cancellationToken)
    {
        if (requests.Count == 0)
        {
            return BadRequestMessage(ApiMessageKey.InvalidRequest);
        }

        logger.LogInformation("Character delete batch received. Total {Total}", requests.Count);
        var result = await commandService.DeleteAsync(requests, cancellationToken);
        return Ok(Localize(result, ApiMessageKey.CharactersDeleted));
    }

    private BadRequestObjectResult BadRequestMessage(ApiMessageKey key)
    {
        var language = languageProvider.GetCurrentLanguage();
        return BadRequest(new ApiErrorResponse
        {
            Code = key.ToString(),
            Message = messageLocalizer.Get(key, language),
            Language = language.ToCultureCode()
        });
    }

    private T Localize<T>(T response, ApiMessageKey key)
        where T : LocalizedResponse
    {
        var language = languageProvider.GetCurrentLanguage();
        response.Message = messageLocalizer.Get(key, language);
        response.Language = language.ToCultureCode();
        return response;
    }
}
