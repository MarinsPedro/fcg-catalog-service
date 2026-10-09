namespace FiapCloudGames.Catalog.Domain.Games;

public interface IGameRepository
{
    Task AddAsync(Game game, CancellationToken cancellationToken);
    Task<Game?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Game>> ListByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    Task<PagedRepositoryResult<Game>> ListAsync(GameQueryParameters parameters, CancellationToken cancellationToken);
}
