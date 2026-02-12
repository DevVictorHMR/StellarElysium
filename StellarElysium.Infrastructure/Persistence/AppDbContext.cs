using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Domain.Entities;

namespace StellarElysium.Infrastructure.Persistence;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ILogger<AppDbContext> logger) : DbContext(options)
{
    public DbSet<ContaGenshin> ContasGenshin => Set<ContaGenshin>();
    public DbSet<Desejo> Desejos => Set<Desejo>();

    public override int SaveChanges()
    {
        LogarAlteracoes();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        LogarAlteracoes();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContaGenshin>()
            .HasMany(conta => conta.Desejos)
            .WithOne(desejo => desejo.ContaGenshin)
            .HasForeignKey(desejo => desejo.ContaGenshinId);

        modelBuilder.Entity<Desejo>()
            .HasIndex(desejo => new { desejo.ContaGenshinId, desejo.PullId })
            .IsUnique();

        modelBuilder.Entity<Desejo>()
            .HasIndex(desejo => new { desejo.ContaGenshinId, desejo.GachaType, desejo.DataHoraDesejo });

        modelBuilder.Entity<ContaGenshin>()
            .HasIndex(conta => new { conta.Uid, conta.Servidor })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    private void LogarAlteracoes()
    {
        if (!ChangeTracker.HasChanges())
        {
            return;
        }

        var total = ChangeTracker
            .Entries()
            .Count(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        if (total > 0)
        {
            logger.LogInformation("Persistindo {Total} alteracoes no banco.", total);
        }
    }
}
