using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;

namespace FiapCloudGames.Catalog.Application.Features.Games.Update;

public sealed class UpdateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork)
{
    public async Task<GameResult> ExecuteAsync(
        Guid id,
        UpdateGameInput input,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Jogo não encontrado");

        game.ChangeDetails(input.Title, input.Description, input.Category, input.BasePrice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CatalogApplicationMappings.ToResult(game);
    }
}
