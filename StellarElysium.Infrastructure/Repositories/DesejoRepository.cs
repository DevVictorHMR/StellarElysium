using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Repositories;
using StellarElysium.Domain.Entities;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Utils;
using StellarElysium.Infrastructure.Persistence;

namespace StellarElysium.Infrastructure.Repositories;

public sealed class DesejoRepository(
    AppDbContext context,
    ILogger<DesejoRepository> logger) : IDesejoRepository
{
    public async Task<HashSet<string>> ObterPullIdsExistentesAsync(
        Guid contaGenshinId,
        IEnumerable<string> pullIds,
        CancellationToken cancellationToken)
    {
        var lista = pullIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (lista.Count == 0)
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        var existentes = await context.Desejos
            .AsNoTracking()
            .Where(desejo => desejo.ContaGenshinId == contaGenshinId && lista.Contains(desejo.PullId))
            .Select(desejo => desejo.PullId)
            .ToListAsync(cancellationToken);

        logger.LogInformation("PullIds existentes encontrados: {Total}", existentes.Count);

        return existentes.ToHashSet(StringComparer.Ordinal);
    }

    public Task AdicionarAsync(IEnumerable<Desejo> desejos, CancellationToken cancellationToken)
    {
        var lista = desejos.ToList();
        if (lista.Count == 0)
        {
            return Task.CompletedTask;
        }

        logger.LogInformation("Adicionando {Total} desejos.", lista.Count);
        context.Desejos.AddRange(lista);
        return Task.CompletedTask;
    }

    public async Task SalvarAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Desejo>> ObterPorContaAsync(
        Guid contaGenshinId,
        int? gachaType,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = context.Desejos
            .AsNoTracking()
            .Where(desejo => desejo.ContaGenshinId == contaGenshinId);

        if (gachaType.HasValue)
        {
            query = query.Where(desejo => desejo.GachaType == gachaType.Value);
        }

        return await AplicarPaginacao(query, skip, take)
            .OrderByDescending(desejo => desejo.DataHoraDesejo)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Desejo>> ObterPorContaAsync(Guid contaGenshinId, CancellationToken cancellationToken)
    {
        return await context.Desejos
            .AsNoTracking()
            .Where(desejo => desejo.ContaGenshinId == contaGenshinId)
            .OrderByDescending(desejo => desejo.DataHoraDesejo)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginadoResultado<Desejo>> ObterPorFiltroAsync(
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
        CancellationToken cancellationToken)
    {
        var query = context.Desejos
            .AsNoTracking()
            .Where(desejo => desejo.ContaGenshinId == contaGenshinId);

        if (gachaType.HasValue)
        {
            query = query.Where(desejo => desejo.GachaType == gachaType.Value);
        }

        if (dataInicio.HasValue)
        {
            query = query.Where(desejo => desejo.DataHoraDesejo >= dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            query = query.Where(desejo => desejo.DataHoraDesejo <= dataFim.Value);
        }

        if (rankType.HasValue)
        {
            query = query.Where(desejo => desejo.RankType == rankType.Value);
        }

        if (!string.IsNullOrWhiteSpace(nome))
        {
            var termo = $"%{nome.Trim()}%";
            query = query.Where(desejo => EF.Functions.Like(desejo.NomeItem, termo));
        }

        if (!string.IsNullOrWhiteSpace(tipoItem))
        {
            var termo = $"%{tipoItem.Trim()}%";
            query = query.Where(desejo => EF.Functions.Like(desejo.TipoItem, termo));
        }

        var total = await query.CountAsync(cancellationToken);
        var ordenado = AplicarOrdenacao(query, ordenarPor ?? DesejoOrdenacaoCampo.DataHora, direcao);
        var itens = await AplicarPaginacao(ordenado, skip, take)
            .ToListAsync(cancellationToken);

        return new PaginadoResultado<Desejo>
        {
            Total = total,
            Itens = itens
        };
    }

    private static IQueryable<Desejo> AplicarPaginacao(IQueryable<Desejo> query, int skip, int take)
    {
        if (skip < 0)
        {
            skip = 0;
        }

        if (take <= 0)
        {
            take = 20;
        }

        if (take > 200)
        {
            take = 200;
        }

        return query.Skip(skip).Take(take);
    }

    private static IQueryable<Desejo> AplicarOrdenacao(
        IQueryable<Desejo> query,
        DesejoOrdenacaoCampo ordenarPor,
        OrdenacaoDirecao direcao)
    {
        if (ordenarPor == DesejoOrdenacaoCampo.RankType)
        {
            return direcao == OrdenacaoDirecao.Asc
                ? query.OrderBy(desejo => desejo.RankType)
                : query.OrderByDescending(desejo => desejo.RankType);
        }

        if (ordenarPor == DesejoOrdenacaoCampo.NomeItem)
        {
            return direcao == OrdenacaoDirecao.Asc
                ? query.OrderBy(desejo => desejo.NomeItem)
                : query.OrderByDescending(desejo => desejo.NomeItem);
        }

        if (ordenarPor == DesejoOrdenacaoCampo.GachaType)
        {
            return direcao == OrdenacaoDirecao.Asc
                ? query.OrderBy(desejo => desejo.GachaType)
                : query.OrderByDescending(desejo => desejo.GachaType);
        }

        return direcao == OrdenacaoDirecao.Asc
            ? query.OrderBy(desejo => desejo.DataHoraDesejo)
            : query.OrderByDescending(desejo => desejo.DataHoraDesejo);
    }
}
