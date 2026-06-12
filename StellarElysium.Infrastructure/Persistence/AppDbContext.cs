using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StellarElysium.Domain.Entities.Wishes;

namespace StellarElysium.Infrastructure.Persistence;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ILogger<AppDbContext> logger) : DbContext(options)
{
    public DbSet<GenshinAccount> GenshinAccounts => Set<GenshinAccount>();
    public DbSet<Wish> Wishes => Set<Wish>();

    public override int SaveChanges()
    {
        LogChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        LogChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenshinAccount>()
            .HasMany(account => account.Wishes)
            .WithOne(wish => wish.GenshinAccount)
            .HasForeignKey(wish => wish.GenshinAccountId);

        modelBuilder.Entity<Wish>()
            .HasIndex(wish => new { wish.GenshinAccountId, wish.PullId })
            .IsUnique();

        modelBuilder.Entity<Wish>()
            .HasIndex(wish => new { wish.GenshinAccountId, wish.GachaType, wish.WishTime });

        modelBuilder.Entity<GenshinAccount>()
            .HasIndex(account => new { account.Uid, account.Server })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    private void LogChanges()
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
            logger.LogInformation("Persisting {Total} database changes.", total);
        }
    }
}
