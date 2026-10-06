using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games;

// TODO: aqui foi seguindo o padrão conforme o projeto Identity, mas não seria o correto um nome como "GameMappings"
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
