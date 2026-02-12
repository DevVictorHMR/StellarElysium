using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StellarElysium.Application.Interfaces.Providers;
using StellarElysium.Application.Interfaces.Repositories;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Infrastructure.Mappers;
using StellarElysium.Infrastructure.Persistence;
using StellarElysium.Infrastructure.Repositories;
using StellarElysium.Infrastructure.Services;
using StellarElysium.Infrastructure.Time;

namespace StellarElysium.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<DesejosMapper>();
        services.AddScoped<IContaGenshinRepository, ContaGenshinRepository>();
        services.AddScoped<IDesejoRepository, DesejoRepository>();
        services.AddScoped<IDesejoImportacaoService, DesejoImportacaoService>();
        services.AddScoped<IDesejoConsultaService, DesejoConsultaService>();
        services.AddScoped<IDesejoImportacaoUrlService, DesejoImportacaoUrlService>();
        services.AddHttpClient<IDesejoHistoricoService, DesejoHistoricoService>();

        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("A connection string 'Default' nao foi configurada.");
        }

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}
