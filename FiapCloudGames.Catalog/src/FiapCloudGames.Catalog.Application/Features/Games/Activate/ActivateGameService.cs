using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games.Activate;

public sealed class ActivateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Game não econtrado");

        if (!game.IsActive)
            throw AppException.Conflict("Game já está ativado.");

        game.Activate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
