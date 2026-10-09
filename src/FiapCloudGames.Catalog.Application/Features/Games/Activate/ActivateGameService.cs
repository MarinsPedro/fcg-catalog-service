namespace FiapCloudGames.Catalog.Application.Features.Games.Activate;

public sealed class ActivateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork,
    ILogger<ActivateGameService> logger)
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando ativação do jogo.");

        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Game não econtrado");

        if (game.IsActive)
            throw AppException.Conflict("Game já está ativado.");

        game.Activate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Jogo ativado com sucesso.");
    }
}
