namespace FiapCloudGames.Catalog.Application.Features.Games.Deactivate;

public sealed class DeactivateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork,
    ILogger<DeactivateGameService> logger)
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando desativação do jogo.");

        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Game não econtrado");

        if (!game.IsActive)
            throw AppException.Conflict("game já está desativado.");

        game.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Jogo desativado com sucesso.");
    }
}
