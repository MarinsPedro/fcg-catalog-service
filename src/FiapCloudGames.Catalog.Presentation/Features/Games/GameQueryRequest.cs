namespace FiapCloudGames.Catalog.Presentation.Features.Games;

public sealed class GameQueryRequest : QueryRequestBase
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
}
