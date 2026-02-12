using StellarElysium.Domain.Dtos.Compartilhado;
using StellarElysium.Domain.Dtos.Desejos.Consulta;

namespace StellarElysium.Application.Interfaces.Services;

public interface IDesejoConsultaService
{
    Task<PaginadoResponse<DesejoResponse>> ListarAsync(
        DesejoConsultaRequest request,
        CancellationToken cancellationToken);

    Task<PityResumoResponse> ObterPityAsync(Guid contaGenshinId, CancellationToken cancellationToken);

    Task<PityDetalheResponse> ObterPityDetalheAsync(PityDetalheRequest request, CancellationToken cancellationToken);
}
