namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public sealed class GameQueryInput
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
    public bool OnlyActive { get; init; } = true;
    public int Page { get; init; }
    public int PageSize { get; init; }
}
