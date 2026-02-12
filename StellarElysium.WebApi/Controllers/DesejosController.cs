using Microsoft.AspNetCore.Mvc;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Domain.Dtos.Compartilhado;
using StellarElysium.Domain.Dtos.Desejos.Consulta;
using StellarElysium.Domain.Dtos.Desejos.Importacao;
using StellarElysium.Domain.Enums;

namespace StellarElysium.WebApi.Controllers;

[ApiController]
[Route("api/desejos")]
public sealed class DesejosController(
    IDesejoImportacaoService importacaoService,
    IDesejoImportacaoUrlService importacaoUrlService,
    IDesejoConsultaService consultaService,
    ILogger<DesejosController> logger) : ControllerBase
{
    [HttpPost("importar")]
    public async Task<ActionResult<ImportacaoDesejosResponse>> Importar(
        [FromBody] ImportacaoDesejosRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Requisicao de importacao recebida. Total {Total}", request.Desejos.Count);
        var resultado = await importacaoService.ImportarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    [HttpPost("importar-url")]
    public async Task<ActionResult<ImportacaoDesejosResponse>> ImportarUrl(
        [FromBody] ImportacaoDesejosUrlRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Requisicao de importacao por url recebida. Uid {Uid}", request.Conta.Uid);
        var resultado = await importacaoUrlService.ImportarPorUrlAsync(request, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<ActionResult<PaginadoResponse<DesejoResponse>>> Listar(
        [FromQuery] Guid contaGenshinId,
        [FromQuery] int? gachaType,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int? rankType,
        [FromQuery] string? nome,
        [FromQuery] string? tipoItem,
        [FromQuery] DesejoOrdenacaoCampo? ordenarPor,
        [FromQuery] OrdenacaoDirecao? direcao,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        if (contaGenshinId == Guid.Empty)
        {
            return BadRequest("ContaGenshinId invalido.");
        }

        var request = new DesejoConsultaRequest
        {
            ContaGenshinId = contaGenshinId,
            GachaType = gachaType,
            DataInicio = dataInicio,
            DataFim = dataFim,
            RankType = rankType,
            Nome = nome,
            TipoItem = tipoItem,
            OrdenarPor = ordenarPor,
            Direcao = direcao ?? OrdenacaoDirecao.Desc,
            Skip = skip,
            Take = take
        };

        var desejos = await consultaService.ListarAsync(request, cancellationToken);
        return Ok(desejos);
    }

    [HttpGet("conta/{contaGenshinId:guid}")]
    public async Task<ActionResult<PaginadoResponse<DesejoResponse>>> ListarPorConta(
        Guid contaGenshinId,
        [FromQuery] int? gachaType,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int? rankType,
        [FromQuery] string? nome,
        [FromQuery] string? tipoItem,
        [FromQuery] DesejoOrdenacaoCampo? ordenarPor,
        [FromQuery] OrdenacaoDirecao? direcao,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        if (contaGenshinId == Guid.Empty)
        {
            return BadRequest("ContaGenshinId invalido.");
        }

        var request = new DesejoConsultaRequest
        {
            ContaGenshinId = contaGenshinId,
            GachaType = gachaType,
            DataInicio = dataInicio,
            DataFim = dataFim,
            RankType = rankType,
            Nome = nome,
            TipoItem = tipoItem,
            OrdenarPor = ordenarPor,
            Direcao = direcao ?? OrdenacaoDirecao.Desc,
            Skip = skip,
            Take = take
        };

        var desejos = await consultaService.ListarAsync(request, cancellationToken);
        return Ok(desejos);
    }

    [HttpGet("{contaGenshinId:guid}/pity")]
    public async Task<ActionResult<PityResumoResponse>> ObterPity(
        Guid contaGenshinId,
        CancellationToken cancellationToken)
    {
        if (contaGenshinId == Guid.Empty)
        {
            return BadRequest("ContaGenshinId invalido.");
        }

        var resultado = await consultaService.ObterPityAsync(contaGenshinId, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{contaGenshinId:guid}/pity-detalhe")]
    public async Task<ActionResult<PityDetalheResponse>> ObterPityDetalhe(
        Guid contaGenshinId,
        [FromQuery] int? gachaType,
        [FromQuery] int quantidadeItens = 5,
        CancellationToken cancellationToken = default)
    {
        if (contaGenshinId == Guid.Empty)
        {
            return BadRequest("ContaGenshinId invalido.");
        }

        var request = new PityDetalheRequest
        {
            ContaGenshinId = contaGenshinId,
            GachaType = gachaType,
            QuantidadeItens = quantidadeItens
        };

        var resultado = await consultaService.ObterPityDetalheAsync(request, cancellationToken);
        return Ok(resultado);
    }
}
