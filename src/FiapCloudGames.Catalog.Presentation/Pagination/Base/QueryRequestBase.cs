namespace FiapCloudGames.Catalog.Presentation.Pagination.Base;

public abstract class QueryRequestBase
{
    public int NumberPage { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
