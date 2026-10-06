using FiapCloudGames.Catalog.Application.Features.Games.Update;

namespace FiapCloudGames.Catalog.Presentation.Features.Games;

internal static class UpdateGameHttpMappings
{
    public static UpdateGameInput ToInput(this UpdateGameRequest request) =>
        new(request.Title,
            request.Description,
            request.Category,
            request.BasePrice);
}
