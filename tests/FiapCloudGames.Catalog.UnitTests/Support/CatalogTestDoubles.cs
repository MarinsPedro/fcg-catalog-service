using FiapCloudGames.Catalog.Domain.Pagination;

namespace FiapCloudGames.Catalog.UnitTests.Support;

internal sealed class FakeGameRepository : IGameRepository
{
    internal Game? Game { get; set; }
    internal IEnumerable<Game> Games { get; set; } = Enumerable.Empty<Game>();
    internal Game? AddedGame { get; private set; }

    public Task AddAsync(Game game, CancellationToken cancellationToken)
    {
        AddedGame = game;
        return Task.CompletedTask;
    }

    public Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = Task.FromResult(Game?.Id == id ? Game : null);
        return result;
    }    

    public Task<IReadOnlyList<Game>> ListByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var games = Game is not null && ids.Contains(Game.Id)
            ? new List<Game> { Game }
            : new List<Game>();
        return Task.FromResult((IReadOnlyList<Game>)games);
    }

    public async Task<PagedRepositoryResult<Game>> ListAsync(
        GameQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var result = await Task.FromResult((IReadOnlyList<Game>)Games);

        var pagedResult = new PagedRepositoryResult<Game>
            {
                Records = result,
                TotalRecords = result.Count
            };

        return pagedResult;
    }
}

internal sealed class SpyCatalogUnitOfWork : ICatalogUnitOfWork
{
    internal int SaveChangesCount { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveChangesCount++;
        return Task.FromResult(1);
    }
}

internal sealed class FixedTimeProvider(DateTimeOffset nowUtc) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => nowUtc;
}

internal static class CatalogTestData
{
    internal static readonly DateTimeOffset NowUtc = new(2026, 1, 10, 12, 0, 0, TimeSpan.Zero);
}
