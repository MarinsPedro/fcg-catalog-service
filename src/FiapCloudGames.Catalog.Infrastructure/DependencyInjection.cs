using FiapCloudGames.Catalog.Application;
using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;
using FiapCloudGames.Catalog.Infrastructure.Health;
using FiapCloudGames.Catalog.Infrastructure.Persistence;
using FiapCloudGames.Catalog.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("A connection string 'Database' não foi configurada.");

        services.AddCatalogApplication();
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICatalogUnitOfWork>(provider => provider.GetRequiredService<CatalogDbContext>());
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddHealthChecks().AddCheck<CatalogDatabaseHealthCheck>("catalog-database", tags: ["ready"]);

        return services;
    }
}
