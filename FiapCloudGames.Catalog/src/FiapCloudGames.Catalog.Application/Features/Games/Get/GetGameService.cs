using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games.Get;

public sealed class GetGameService(IGameRepository gameRepository)
{
    public async Task<GameResult> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetAsync(id, cancellationToken);

        return game is null
            ? throw AppException.NotFound("Game não encontrado") 
            : CatalogApplicationMappings.ToResult(game);
    }
}
