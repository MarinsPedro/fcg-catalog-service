namespace FiapCloudGames.Catalog.Domain.Pagination.Base;

public abstract class QueryParametersBase
{
    public int Offset { get; init; }
    public int Limit { get; init; }
}
