using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games.Create;

// TODO: utilizar logger? sim usar... ok, implementar...

public sealed class CreateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork,
    TimeProvider clock)
{
    public async Task<GameResult> ExecuteAsync(
            CreateGameInput input,
            CancellationToken cancellationToken)
    {
        // TODO: validações vão aqui?

        var game = Game.Create(
            input.Title,
            input.Description,
            input.Category,
            input.BasePrice,
            clock.GetUtcNow());

        await gameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CatalogApplicationMappings.ToResult(game);
    }
}
