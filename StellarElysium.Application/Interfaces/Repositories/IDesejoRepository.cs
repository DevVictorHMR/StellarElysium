using StellarElysium.Domain.Entities;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Utils;

namespace StellarElysium.Application.Interfaces.Repositories;

public interface IDesejoRepository
{
    Task<HashSet<string>> ObterPullIdsExistentesAsync(Guid contaGenshinId, IEnumerable<string> pullIds, CancellationToken cancellationToken);
    Task<List<Desejo>> ObterPorContaAsync(Guid contaGenshinId, int? gachaType, int skip, int take, CancellationToken cancellationToken);
    Task<List<Desejo>> ObterPorContaAsync(Guid contaGenshinId, CancellationToken cancellationToken);
    Task<PaginadoResultado<Desejo>> ObterPorFiltroAsync(
        Guid contaGenshinId,
        int? gachaType,
        DateTime? dataInicio,
        DateTime? dataFim,
        int? rankType,
        string? nome,
        string? tipoItem,
        DesejoOrdenacaoCampo? ordenarPor,
        OrdenacaoDirecao direcao,
        int skip,
        int take,
        CancellationToken cancellationToken);
    Task AdicionarAsync(IEnumerable<Desejo> desejos, CancellationToken cancellationToken);
    Task SalvarAsync(CancellationToken cancellationToken);
}
