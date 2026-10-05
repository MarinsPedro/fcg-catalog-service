using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games.Deactivate;

public sealed class DeactivateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Game não econtrado");

        if (!game.IsActive)
            throw AppException.Conflict("game já está desativado.");

        game.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
