using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Domain.Games;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Catalog.Application.Features.Games.Get;

public sealed class GetGameService(IGameRepository gameRepository, ILogger<GetGameService> logger)
{
    public async Task<GameResult> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Localizando jogo com id {Id}", id);

        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Jogo não encontrado");

        logger.LogInformation("Jogo encontrado: {GameId}", game.Id);

        return CatalogApplicationMappings.ToResult(game);
    }
}
