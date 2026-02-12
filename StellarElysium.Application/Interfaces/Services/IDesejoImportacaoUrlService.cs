using StellarElysium.Domain.Dtos.Desejos.Importacao;

namespace StellarElysium.Application.Interfaces.Services;

public interface IDesejoImportacaoUrlService
{
    Task<ImportacaoDesejosResponse> ImportarPorUrlAsync(
        ImportacaoDesejosUrlRequest request,
        CancellationToken cancellationToken);
}
