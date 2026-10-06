namespace FiapCloudGames.Catalog.Presentation.Features.Games;

public sealed class GameQueryRequest
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
