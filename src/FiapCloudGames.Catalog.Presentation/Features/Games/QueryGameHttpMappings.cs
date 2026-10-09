using FiapCloudGames.Catalog.Application.Features.Games.List;

namespace FiapCloudGames.Catalog.Presentation.Features.Games;

internal static class QueryGameHttpMappings
{
    public static GameQueryInput ToInput(this GameQueryRequest request) =>
        new()
        {
            Title = request.Title,
            MaxPrice = request.MaxPrice,
            OnlyActive = true,
            NumberPage = request.NumberPage,
            PageSize = request.PageSize
        };
}
