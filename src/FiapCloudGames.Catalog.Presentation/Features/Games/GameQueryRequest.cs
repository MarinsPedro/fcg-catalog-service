namespace FiapCloudGames.Catalog.Presentation.Features.Games;

public sealed class GameQueryRequest : QueryRequestBase
{
    public string? Title { get; init; }
    
    [Range(0, 9999999999)]
    public int? MaxPrice { get; init; }
}
