namespace FiapCloudGames.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
    {
        services.AddScoped<ActivateGameService>();
        services.AddScoped<CreateGameService>();
        services.AddScoped<DeactivateGameService>();
        services.AddScoped<GetGameService>();
        services.AddScoped<ListGameService>();
        services.AddScoped<UpdateGameService>();

        return services;
    }
}
