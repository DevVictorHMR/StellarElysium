using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace StellarElysium.Infrastructure.Persistence;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true);

        var webApiPaths = new[]
        {
            Path.Combine(basePath, "StellarElysium.WebApi"),
            Path.GetFullPath(Path.Combine(basePath, "..", "StellarElysium.WebApi"))
        };

        foreach (var webApiPath in webApiPaths)
        {
            if (!Directory.Exists(webApiPath))
            {
                continue;
            }

            configBuilder
                .AddJsonFile(Path.Combine(webApiPath, "appsettings.json"), optional: true)
                .AddJsonFile(Path.Combine(webApiPath, "appsettings.Development.json"), optional: true);
        }

        configBuilder.AddEnvironmentVariables();

        var configuration = configBuilder.Build();
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The connection string 'Default' was not configured.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options, NullLogger<AppDbContext>.Instance);
    }
}
