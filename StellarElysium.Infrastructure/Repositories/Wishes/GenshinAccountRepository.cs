using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Domain.Entities.Wishes;
using StellarElysium.Domain.Enums.Wishes;
using StellarElysium.Infrastructure.Persistence;

namespace StellarElysium.Infrastructure.Repositories.Wishes;

public sealed class GenshinAccountRepository(
    AppDbContext context,
    ILogger<GenshinAccountRepository> logger) : IGenshinAccountRepository
{
    public async Task<GenshinAccount?> GetByUidAsync(string uid, GenshinServer server, CancellationToken cancellationToken)
    {
        logger.LogInformation("Searching account. Uid {Uid} Server {Server}", uid, server);

        return await context.GenshinAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                account => account.Uid == uid && account.Server == server,
                cancellationToken);
    }

    public async Task<GenshinAccount> CreateAsync(GenshinAccount account, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating account. Uid {Uid} Server {Server}", account.Uid, account.Server);

        context.GenshinAccounts.Add(account);
        await context.SaveChangesAsync(cancellationToken);

        return account;
    }
}
