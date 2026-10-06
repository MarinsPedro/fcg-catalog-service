using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Catalog.Application.Features.Games.Create;

public sealed class CreateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork,
    TimeProvider clock,
    ILogger<CreateGameService> logger)
{
    public async Task<GameResult> ExecuteAsync(CreateGameInput input, CancellationToken cancellationToken)
    {
        // TODO: validações vão aqui? sim, validações sempre vão na service.

        logger.LogInformation("Criando registro do jogo.");

        var game = Game.Create(
            input.Title,
            input.Description,
            input.Category,
            input.BasePrice,
            clock.GetUtcNow());

        await gameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Jogo {GameId} criado com sucesso", game.Id);
        return CatalogApplicationMappings.ToResult(game);
    }
}
