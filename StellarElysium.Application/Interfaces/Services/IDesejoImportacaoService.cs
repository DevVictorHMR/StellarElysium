using StellarElysium.Domain.Dtos.Desejos.Importacao;

namespace StellarElysium.Application.Interfaces.Services;

public interface IDesejoImportacaoService
{
    Task<ImportacaoDesejosResponse> ImportarAsync(ImportacaoDesejosRequest request, CancellationToken cancellationToken);
}
