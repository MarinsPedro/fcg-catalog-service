namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public sealed class GameQueryInput : QueryInputBase
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
    public bool OnlyActive { get; init; } = true;
}
