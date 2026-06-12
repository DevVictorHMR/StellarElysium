using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StellarElysium.Application.Interfaces.Providers;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Infrastructure.Mappers.Wishes;
using StellarElysium.Infrastructure.Persistence;
using StellarElysium.Infrastructure.Repositories.Wishes;
using StellarElysium.Infrastructure.Services.Wishes;
using StellarElysium.Infrastructure.Providers;

namespace StellarElysium.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<WishesMapper>();
        services.AddScoped<IGenshinAccountRepository, GenshinAccountRepository>();
        services.AddScoped<IWishRepository, WishRepository>();
        services.AddScoped<IWishImportService, WishImportService>();
        services.AddScoped<IWishQueryService, WishQueryService>();
        services.AddScoped<IWishPityService, WishPityService>();
        services.AddScoped<IWishHistoryUrlImportService, WishHistoryUrlImportService>();
        services.AddHttpClient<IWishHistoryService, WishHistoryService>();

        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The connection string 'Default' was not configured.");
        }

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}
