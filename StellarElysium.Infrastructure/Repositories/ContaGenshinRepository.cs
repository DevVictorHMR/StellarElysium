using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories;
using StellarElysium.Domain.Entities;
using StellarElysium.Domain.Enums;
using StellarElysium.Infrastructure.Persistence;

namespace StellarElysium.Infrastructure.Repositories;

public sealed class ContaGenshinRepository(
    AppDbContext context,
    ILogger<ContaGenshinRepository> logger) : IContaGenshinRepository
{
    public async Task<ContaGenshin?> ObterPorUidAsync(string uid, ServidorGenshin servidor, CancellationToken cancellationToken)
    {
        logger.LogInformation("Buscando conta. Uid {Uid} Servidor {Servidor}", uid, servidor);

        return await context.ContasGenshin
            .AsNoTracking()
            .FirstOrDefaultAsync(
                conta => conta.Uid == uid && conta.Servidor == servidor,
                cancellationToken);
    }

    public async Task<ContaGenshin> CriarAsync(ContaGenshin conta, CancellationToken cancellationToken)
    {
        logger.LogInformation("Criando conta. Uid {Uid} Servidor {Servidor}", conta.Uid, conta.Servidor);

        context.ContasGenshin.Add(conta);
        await context.SaveChangesAsync(cancellationToken);

        return conta;
    }
}
