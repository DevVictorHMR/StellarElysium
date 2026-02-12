using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Domain.Dtos.Desejos.Importacao;

namespace StellarElysium.Infrastructure.Services;

public sealed class DesejoImportacaoUrlService(
    IDesejoHistoricoService historicoService,
    IDesejoImportacaoService importacaoService,
    ILogger<DesejoImportacaoUrlService> logger) : IDesejoImportacaoUrlService
{
    public async Task<ImportacaoDesejosResponse> ImportarPorUrlAsync(
        ImportacaoDesejosUrlRequest request,
        CancellationToken cancellationToken)
    {
        var gachaTypes = request.GachaTypes ?? [];
        logger.LogInformation("Importacao por url iniciada. Uid {Uid} Tipos {Tipos}",
            request.Conta.Uid,
            gachaTypes.Count == 0 ? "padrao" : string.Join(",", gachaTypes));

        var desejos = await historicoService.BuscarAsync(request.Url, gachaTypes, cancellationToken);
        var importacao = new ImportacaoDesejosRequest
        {
            Conta = request.Conta,
            Desejos = desejos.ToList()
        };

        var resultado = await importacaoService.ImportarAsync(importacao, cancellationToken);
        logger.LogInformation("Importacao por url concluida. Importados {Importados} Ignorados {Ignorados}",
            resultado.Importados,
            resultado.Ignorados);

        return resultado;
    }
}
