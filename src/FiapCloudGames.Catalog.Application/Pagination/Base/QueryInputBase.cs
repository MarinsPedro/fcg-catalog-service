namespace FiapCloudGames.Catalog.Application.Pagination.Base;

public abstract class QueryInputBase
{
    public int NumberPage { get; init; }
    public int PageSize { get; init; }
}
