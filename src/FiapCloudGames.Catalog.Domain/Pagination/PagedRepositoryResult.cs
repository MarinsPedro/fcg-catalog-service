namespace FiapCloudGames.Catalog.Domain.Pagination;

public sealed class PagedRepositoryResult<T> where T: class
{
    public IReadOnlyList<T> Records { get; init; } = Array.Empty<T>();
    public int TotalRecords { get; init; }
}
