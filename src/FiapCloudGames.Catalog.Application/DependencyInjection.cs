using FiapCloudGames.Catalog.Application.Features.Games.Activate;
using FiapCloudGames.Catalog.Application.Features.Games.Create;
using FiapCloudGames.Catalog.Application.Features.Games.Deactivate;
using FiapCloudGames.Catalog.Application.Features.Games.Get;
using FiapCloudGames.Catalog.Application.Features.Games.Update;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
    {
        services.AddScoped<ActivateGameService>();
        services.AddScoped<CreateGameService>();
        services.AddScoped<DeactivateGameService>();
        services.AddScoped<GetGameService>();
        services.AddScoped<UpdateGameService>();

        return services;
    }
}
