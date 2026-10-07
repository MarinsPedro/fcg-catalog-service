using FiapCloudGames.BuildingBlocks.Exceptions;
using FiapCloudGames.Catalog.Domain.Games;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public sealed class ListGameService(
    IGameRepository gameRepository,
    ILogger<ListGameService> logger)
{
    public async Task<IReadOnlyList<GameResult>> ExecuteAsync(GameQueryInput input, CancellationToken cancellationToken)
    {
        CheckInput(input);

        logger.LogInformation("Listando jogos (com filtro e paginação aplicados))");

        var result = (await gameRepository
            .ListAsync(input.ToInput(), cancellationToken))
            .Select(CatalogApplicationMappings.ToResult)
            .ToArray();

        logger.LogInformation("Total de {Count} jogos listados com sucesso", result.Length);

        return result;
    }

    private static void CheckInput(GameQueryInput input)
    {
        if (input.Page < 1)
            throw AppException.Conflict("O valor de página deve ser maior ou igual a 1.");

        if (input.PageSize < 1)
            throw AppException.Conflict("O valor do tamanho da página deve ser maior ou igual a 1.");

        if (input.PageSize > 100)
            throw AppException.Conflict("O valor do tamanho da página deve ser menor ou igual a 100.");
    }
}
