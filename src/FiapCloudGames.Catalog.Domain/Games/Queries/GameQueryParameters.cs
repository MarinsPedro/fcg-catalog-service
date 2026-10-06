namespace FiapCloudGames.Catalog.Domain.Games.Queries;

public sealed class GameQueryParameters
{
    public string? Title { get; init; }
    public int? MaxPrice { get; init; }
    public bool OnlyActive { get; init; }
    public int Offset { get; init; }
    public int Limit { get; init; }
}
