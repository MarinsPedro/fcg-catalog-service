using FiapCloudGames.Catalog.Domain.Games;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public sealed class ListGameService(
    IGameRepository gameRepository,
    ILogger<ListGameService> logger)
{
    public async Task<IReadOnlyList<GameResult>> ExecuteAsync(bool onlyActive, CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando jogos (apenas ativos)");

        var result = (await gameRepository
            .ListAsync(onlyActive, cancellationToken))
            .Select(CatalogApplicationMappings.ToResult)
            .ToArray();

        logger.LogInformation("Total de {Count} jogos listados com sucesso", result.Length);

        return result;
    }
}
