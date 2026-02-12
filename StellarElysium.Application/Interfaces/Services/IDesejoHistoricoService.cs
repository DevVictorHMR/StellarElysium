using StellarElysium.Domain.Dtos.Desejos.Importacao;

namespace StellarElysium.Application.Interfaces.Services;

public interface IDesejoHistoricoService
{
    Task<IReadOnlyList<DesejoImportacaoRequest>> BuscarAsync(
        string url,
        IReadOnlyCollection<int> gachaTypes,
        CancellationToken cancellationToken);
}
