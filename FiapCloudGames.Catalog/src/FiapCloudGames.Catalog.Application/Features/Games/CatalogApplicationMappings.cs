using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games;

internal static class CatalogApplicationMappings
{
    public static GameResult ToResult(Game game) =>
        new(game.Id,
            game.Title,
            game.Description,
            game.Category,
            game.BasePrice.Amount,
            game.IsActive);
}
