using ChileGeo.DataAccess.Infrastructure;
using ChileGeo.DataAccess.Repositories;
using ChileGeo.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChileGeo.DataAccess;

/// <summary>Composition-root helper that registers the data-access layer abstractions and implementations.</summary>
public static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<IComunaRepository, ComunaRepository>();
        return services;
    }
}
