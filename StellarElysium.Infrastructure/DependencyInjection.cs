using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StellarElysium.Application.Interfaces.Localization;
using StellarElysium.Application.Interfaces.Providers;
using StellarElysium.Application.Interfaces.Repositories.Characters;
using StellarElysium.Application.Interfaces.Repositories.Wishes;
using StellarElysium.Application.Interfaces.Services.Characters;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Infrastructure.Mappers.Characters;
using StellarElysium.Infrastructure.Mappers.Wishes;
using StellarElysium.Infrastructure.Persistence;
using StellarElysium.Infrastructure.Repositories.Characters;
using StellarElysium.Infrastructure.Repositories.Wishes;
using StellarElysium.Infrastructure.Services.Characters;
using StellarElysium.Infrastructure.Services.Localization;
using StellarElysium.Infrastructure.Services.Wishes;
using StellarElysium.Infrastructure.Providers;

namespace StellarElysium.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IApiMessageLocalizer, ApiMessageLocalizer>();
        services.AddSingleton<WishesMapper>();
        services.AddSingleton<CharactersMapper>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<ICharacterCommandService, CharacterCommandService>();
        services.AddScoped<ICharacterQueryService, CharacterQueryService>();
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
