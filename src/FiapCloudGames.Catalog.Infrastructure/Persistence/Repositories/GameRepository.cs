namespace FiapCloudGames.Catalog.Infrastructure.Persistence.Repositories;

internal sealed class GameRepository(CatalogDbContext dbContext) : IGameRepository
{
    public async Task AddAsync(Game game, CancellationToken cancellationToken)
        => await dbContext.Games.AddAsync(game, cancellationToken);

    public async Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Games.SingleOrDefaultAsync(game => game.Id == id, cancellationToken);
        return result;
    }

    public async Task<IReadOnlyList<Game>> ListByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
            return [];

        return await dbContext.Games
            .AsNoTracking()
            .Where(game => ids.Contains(game.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedRepositoryResult<Game>> ListAsync(GameQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = dbContext.Games.AsNoTracking();

        if (parameters.Title is not null)
            query = query.Where(game => EF.Functions.Like(game.Title, $"%{parameters.Title}%")); // TODO: ignorar case sensitive?

        if (parameters.MaxPrice is not null)
            query = query.Where(game => game.BasePrice <= GamePrice.Create(parameters.MaxPrice.Value));

        if (parameters.OnlyActive)
            query = query.Where(game => game.IsActive);

        var totalRecords = await query.CountAsync(cancellationToken);

        var result = await query
                        .OrderBy(game => game.Title)
                        .Skip(parameters.Offset)
                        .Take(parameters.Limit)
                        .ToListAsync(cancellationToken);

        var pagedResult = new PagedRepositoryResult<Game>
        {
            Records = result,
            TotalRecords = totalRecords
        };

        return pagedResult;
    }
}
