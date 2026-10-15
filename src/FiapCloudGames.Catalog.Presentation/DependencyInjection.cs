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
                    .RequireScopes("catalog.games.create"));
            options.AddPolicy(
                CatalogPolicies.GamesRead,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireScopes("catalog.games.read"));
            options.AddPolicy(
                CatalogPolicies.GamesWrite,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireServiceToken()
                    .RequireScopes("catalog.games.write"));
        });

        return services;
    }
}
