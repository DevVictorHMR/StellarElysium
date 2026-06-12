using StellarElysium.Domain.Entities.Wishes;
using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.Application.Interfaces.Repositories.Wishes;

public interface IGenshinAccountRepository
{
    Task<GenshinAccount?> GetByUidAsync(string uid, GenshinServer server, CancellationToken cancellationToken);
    Task<GenshinAccount> CreateAsync(GenshinAccount account, CancellationToken cancellationToken);
}
