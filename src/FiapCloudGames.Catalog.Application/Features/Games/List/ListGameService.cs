namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public sealed class ListGameService(
    IGameRepository gameRepository,
    ILogger<ListGameService> logger)
{
    public async Task<PagedResult<GameResult>> ExecuteAsync(GameQueryInput input, CancellationToken cancellationToken)
    {
        CheckInput(input);

        logger.LogInformation("Listando jogos (com filtro e paginação aplicados))");

        var result = (await gameRepository
            .ListAsync(input.ToInput(), cancellationToken));

        logger.LogInformation("Total de {Count} jogos listados com sucesso", result.TotalRecords);

        return new PagedResult<GameResult>
        {
            Items = result.Records.Select(CatalogApplicationMappings.ToResult).ToList(),
            Page = input.NumberPage,
            PageSize = input.PageSize,
            TotalItems = result.TotalRecords
        };
    }

    // TODO: mover as validações de paginação para umua classe abstrata.
    private static void CheckInput(GameQueryInput input)
    {
        if (input.NumberPage < 1)
            throw AppException.Conflict("O valor de página deve ser maior ou igual a 1.");

        if (input.PageSize < 1)
            throw AppException.Conflict("O valor do tamanho da página deve ser maior ou igual a 1.");

        if (input.PageSize > 100)
            throw AppException.Conflict("O valor do tamanho da página deve ser menor ou igual a 100.");
    }
}
