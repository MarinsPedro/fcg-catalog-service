namespace FiapCloudGames.Catalog.Domain.Games.Queries;

public sealed class GameQueryParameters : QueryParametersBase
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
    public bool OnlyActive { get; init; }    
}
