using StellarElysium.Domain.Entities;
using StellarElysium.Domain.Enums;

namespace StellarElysium.Application.Interfaces.Repositories;

public interface IContaGenshinRepository
{
    Task<ContaGenshin?> ObterPorUidAsync(string uid, ServidorGenshin servidor, CancellationToken cancellationToken);
    Task<ContaGenshin> CriarAsync(ContaGenshin conta, CancellationToken cancellationToken);
}
