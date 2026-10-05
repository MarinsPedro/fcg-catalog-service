using FiapCloudGames.Catalog.Application.Features.Games.Create;

namespace FiapCloudGames.Catalog.Presentation.Features.Games;

internal static class CreateGameHttpMappings
{
    public static CreateGameInput ToInput(this CreateGameRequest request) =>
        new (request.Title,
            request.Description,
            request.Category,
            request.BasePrice);
}
