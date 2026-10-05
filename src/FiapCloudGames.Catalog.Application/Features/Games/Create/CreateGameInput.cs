namespace FiapCloudGames.Catalog.Application.Features.Games.Create;

public sealed record CreateGameInput(
        string Title,
        string Description,
        string Category,
        decimal BasePrice);
