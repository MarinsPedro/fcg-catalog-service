namespace FiapCloudGames.Catalog.Application.Features.Games.Update;

public sealed class UpdateGameService(
    IGameRepository gameRepository,
    ICatalogUnitOfWork unitOfWork,
    ILogger<UpdateGameService> logger)
{
    public async Task<GameResult> ExecuteAsync(
        Guid id,
        UpdateGameInput input,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Atualizando jogo com id {Id}", id);

        var game = await gameRepository.GetAsync(id, cancellationToken)
            ?? throw AppException.NotFound("Jogo não encontrado");

        game.ChangeDetails(input.Title, input.Description, input.Category, input.BasePrice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Jogo atualizado com sucesso.");
        return CatalogApplicationMappings.ToResult(game);
    }
}
