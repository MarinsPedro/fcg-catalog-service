using FiapCloudGames.BuildingBlocks.Authentication;
using FiapCloudGames.Catalog.Presentation.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Catalog.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogPresentation(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                CatalogPolicies.GamesCreate,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.games.create"));
            options.AddPolicy(
                CatalogPolicies.GamesRead,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.games.read"));
            options.AddPolicy(
                CatalogPolicies.GamesWrite,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.games.write"));
            options.AddPolicy(
                CatalogPolicies.ClientsCreate,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.clients.create"));
            options.AddPolicy(
                CatalogPolicies.ClientsRead,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.clients.read"));
            options.AddPolicy(
                CatalogPolicies.ClientsWrite,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.clients.write"));
            options.AddPolicy(
                CatalogPolicies.AuditRead,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireClaim(FiapClaimTypes.Scope, "catalog.audit.read"));
        });

        return services;
    }
}
