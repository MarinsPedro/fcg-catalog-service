namespace FiapCloudGames.Catalog.Application.Features.Games.Update;

public sealed record UpdateGameInput(
    string Title,
    string Description,
    string Category,
    decimal BasePrice);
