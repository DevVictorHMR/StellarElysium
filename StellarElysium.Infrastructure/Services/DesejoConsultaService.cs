using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Domain.Dtos.Compartilhado;
using StellarElysium.Domain.Dtos.Desejos.Consulta;
using StellarElysium.Infrastructure.Mappers;

namespace StellarElysium.Infrastructure.Services;

public sealed class DesejoConsultaService(
    IDesejoRepository desejoRepository,
    DesejosMapper mapper,
    ILogger<DesejoConsultaService> logger) : IDesejoConsultaService
{
    public async Task<PaginadoResponse<DesejoResponse>> ListarAsync(
        DesejoConsultaRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando desejos. ContaId {ContaId}", request.ContaGenshinId);

        var skip = request.Skip < 0 ? 0 : request.Skip;
        var take = request.Take <= 0 ? 20 : request.Take;
        if (take > 200)
        {
            take = 200;
        }

        var resultado = await desejoRepository.ObterPorFiltroAsync(
            request.ContaGenshinId,
            request.GachaType,
            request.DataInicio,
            request.DataFim,
            request.RankType,
            request.Nome,
            request.TipoItem,
            request.OrdenarPor,
            request.Direcao,
            skip,
            take,
            cancellationToken);

        return new PaginadoResponse<DesejoResponse>
        {
            Total = resultado.Total,
            Skip = skip,
            Take = take,
            Itens = resultado.Itens.Select(mapper.Map).ToList()
        };
    }

    public async Task<PityResumoResponse> ObterPityAsync(Guid contaGenshinId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calculando pity. ContaId {ContaId}", contaGenshinId);

        var desejos = await desejoRepository.ObterPorContaAsync(contaGenshinId, cancellationToken);
        var grupos = desejos
            .GroupBy(desejo => desejo.GachaType)
            .OrderBy(grupo => grupo.Key)
            .ToList();

        var resposta = new PityResumoResponse
        {
            ContaGenshinId = contaGenshinId
        };

        foreach (var grupo in grupos)
        {
            var ordenados = grupo
                .OrderByDescending(desejo => desejo.DataHoraDesejo)
                .ToList();

            var pityCinco = 0;
            DateTime? ultimoCinco = null;
            string? ultimoCincoNome = null;
            foreach (var desejo in ordenados)
            {
                if (desejo.RankType == 5)
                {
                    ultimoCinco = desejo.DataHoraDesejo;
                    ultimoCincoNome = desejo.NomeItem;
                    break;
                }

                pityCinco++;
            }

            var pityQuatro = 0;
            DateTime? ultimoQuatro = null;
            string? ultimoQuatroNome = null;
            foreach (var desejo in ordenados)
            {
                if (desejo.RankType == 4)
                {
                    ultimoQuatro = desejo.DataHoraDesejo;
                    ultimoQuatroNome = desejo.NomeItem;
                    break;
                }

                pityQuatro++;
            }

            resposta.Banners.Add(new PityBannerResponse
            {
                GachaType = grupo.Key,
                Total = ordenados.Count,
                PityCinco = ordenados.Count == 0 ? 0 : pityCinco,
                PityQuatro = ordenados.Count == 0 ? 0 : pityQuatro,
                UltimoCinco = ultimoCinco,
                UltimoQuatro = ultimoQuatro,
                UltimoCincoNome = ultimoCincoNome,
                UltimoQuatroNome = ultimoQuatroNome
            });
        }

        return resposta;
    }

    public async Task<PityDetalheResponse> ObterPityDetalheAsync(PityDetalheRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Calculando pity detalhado. ContaId {ContaId}", request.ContaGenshinId);

        var desejos = await desejoRepository.ObterPorContaAsync(request.ContaGenshinId, cancellationToken);
        var query = desejos.AsEnumerable();

        if (request.GachaType.HasValue)
        {
            query = query.Where(desejo => desejo.GachaType == request.GachaType.Value);
        }

        var agrupados = query
            .GroupBy(desejo => desejo.GachaType)
            .OrderBy(grupo => grupo.Key)
            .ToList();

        var resposta = new PityDetalheResponse
        {
            ContaGenshinId = request.ContaGenshinId
        };

        foreach (var grupo in agrupados)
        {
            var ordenados = grupo
                .OrderByDescending(desejo => desejo.DataHoraDesejo)
                .ToList();

            var pityCinco = 0;
            foreach (var desejo in ordenados)
            {
                if (desejo.RankType == 5)
                {
                    break;
                }

                pityCinco++;
            }

            var pityQuatro = 0;
            foreach (var desejo in ordenados)
            {
                if (desejo.RankType == 4)
                {
                    break;
                }

                pityQuatro++;
            }

            var limite = Math.Clamp(request.QuantidadeItens, 1, 20);

            var ultimosCinco = ordenados
                .Where(desejo => desejo.RankType == 5)
                .Take(limite)
                .Select(desejo => new PityItemResponse
                {
                    NomeItem = desejo.NomeItem,
                    RankType = desejo.RankType,
                    DataHoraDesejo = desejo.DataHoraDesejo
                })
                .ToList();

            var ultimosQuatro = ordenados
                .Where(desejo => desejo.RankType == 4)
                .Take(limite)
                .Select(desejo => new PityItemResponse
                {
                    NomeItem = desejo.NomeItem,
                    RankType = desejo.RankType,
                    DataHoraDesejo = desejo.DataHoraDesejo
                })
                .ToList();

            resposta.Banners.Add(new PityBannerDetalheResponse
            {
                GachaType = grupo.Key,
                PityCinco = ordenados.Count == 0 ? 0 : pityCinco,
                PityQuatro = ordenados.Count == 0 ? 0 : pityQuatro,
                UltimosCinco = ultimosCinco,
                UltimosQuatro = ultimosQuatro
            });
        }

        return resposta;
    }
}
