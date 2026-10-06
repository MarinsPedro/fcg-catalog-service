using FiapCloudGames.Catalog.Domain.Games;
using FiapCloudGames.Catalog.Domain.Games.Queries;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Catalog.Infrastructure.Persistence.Repositories;

internal sealed class GameRepository(CatalogDbContext dbContext) : IGameRepository
{
    public async Task AddAsync(Game game, CancellationToken cancellationToken)
        => await dbContext.Games.AddAsync(game, cancellationToken);

    public Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Games.SingleOrDefaultAsync(game => game.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Game>> ListByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
            return [];

        return await dbContext.Games
            .AsNoTracking()
            .Where(game => ids.Contains(game.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Game>> ListAsync(GameQueryParameters parameters, CancellationToken cancellationToken)
    {
        var query = dbContext.Games.AsNoTracking();

        if (parameters.Title is not null)
            query = query.Where(game => EF.Functions.Like(game.Title, $"%{parameters.Title}%"));

        if (parameters.MaxPrice is not null)
            query = query.Where(game => game.BasePrice <= GamePrice.Create(parameters.MaxPrice.Value));

        if (parameters.OnlyActive)
            query = query.Where(game => game.IsActive);

        return await query
            .OrderBy(game => game.Title)
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToListAsync(cancellationToken);
    }
}
