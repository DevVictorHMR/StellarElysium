using System.Globalization;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Domain.Dtos.Desejos.Importacao;
using StellarElysium.Domain.Entities;
using StellarElysium.Infrastructure.Mappers;

namespace StellarElysium.Infrastructure.Services;

public sealed class DesejoImportacaoService(
    IContaGenshinRepository contaRepository,
    IDesejoRepository desejoRepository,
    DesejosMapper mapper,
    ILogger<DesejoImportacaoService> logger) : IDesejoImportacaoService
{
    public async Task<ImportacaoDesejosResponse> ImportarAsync(
        ImportacaoDesejosRequest request,
        CancellationToken cancellationToken)
    {
        var totalRecebido = request.Desejos.Count;
        logger.LogInformation("Importacao iniciada. Uid {Uid} Total {Total}", request.Conta.Uid, totalRecebido);

        var conta = await contaRepository.ObterPorUidAsync(request.Conta.Uid, request.Conta.Servidor, cancellationToken);
        if (conta is null)
        {
            conta = mapper.Map(request.Conta);
            conta = await contaRepository.CriarAsync(conta, cancellationToken);
            logger.LogInformation("Conta criada. ContaId {ContaId}", conta.Id);
        }

        var desejosConvertidos = new List<Desejo>(totalRecebido);
        var invalidos = 0;

        foreach (var item in request.Desejos)
        {
            if (!TryMap(item, conta.Id, out var desejo))
            {
                invalidos++;
                continue;
            }

            desejosConvertidos.Add(desejo);
        }

        if (desejosConvertidos.Count == 0)
        {
            logger.LogInformation("Importacao encerrada sem desejos validos.");
            return new ImportacaoDesejosResponse
            {
                ContaGenshinId = conta.Id,
                TotalRecebido = totalRecebido,
                Importados = 0,
                Ignorados = totalRecebido
            };
        }

        var pullIds = desejosConvertidos
            .Select(desejo => desejo.PullId)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var existentes = await desejoRepository.ObterPullIdsExistentesAsync(conta.Id, pullIds, cancellationToken);
        var novos = new List<Desejo>();
        var vistos = new HashSet<string>(existentes, StringComparer.Ordinal);

        foreach (var desejo in desejosConvertidos)
        {
            if (!vistos.Add(desejo.PullId))
            {
                continue;
            }

            novos.Add(desejo);
        }

        await desejoRepository.AdicionarAsync(novos, cancellationToken);
        await desejoRepository.SalvarAsync(cancellationToken);

        var ignorados = totalRecebido - novos.Count;
        if (invalidos > 0)
        {
            logger.LogWarning("Desejos invalidos ignorados: {Total}", invalidos);
        }

        logger.LogInformation("Importacao concluida. Novos {Novos} Ignorados {Ignorados}", novos.Count, ignorados);

        return new ImportacaoDesejosResponse
        {
            ContaGenshinId = conta.Id,
            TotalRecebido = totalRecebido,
            Importados = novos.Count,
            Ignorados = ignorados
        };
    }

    private static bool TryMap(DesejoImportacaoRequest item, Guid contaId, out Desejo desejo)
    {
        desejo = new Desejo();

        if (string.IsNullOrWhiteSpace(item.PullId))
        {
            return false;
        }

        if (!int.TryParse(item.GachaType, NumberStyles.Integer, CultureInfo.InvariantCulture, out var gachaType))
        {
            return false;
        }

        if (!int.TryParse(item.RankType, NumberStyles.Integer, CultureInfo.InvariantCulture, out var rankType))
        {
            return false;
        }

        if (!int.TryParse(item.Quantidade, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantidade))
        {
            return false;
        }

        if (!DateTime.TryParseExact(
                item.DataHora,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dataHora))
        {
            if (!DateTime.TryParse(item.DataHora, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataHora))
            {
                return false;
            }
        }

        desejo = new Desejo
        {
            ContaGenshinId = contaId,
            PullId = item.PullId,
            GachaType = gachaType,
            ItemId = item.ItemId,
            NomeItem = item.NomeItem,
            TipoItem = item.TipoItem,
            RankType = rankType,
            Quantidade = quantidade,
            DataHoraDesejo = dataHora
        };

        return true;
    }
}
