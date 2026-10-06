using FiapCloudGames.Catalog.Domain.Games.Queries;

namespace FiapCloudGames.Catalog.Application.Features.Games.List;

public static class GameQueryMapping
{
    public static GameQueryParameters ToInput(this GameQueryInput input) =>
        new()
        {
            Title = input.Title,
            MaxPrice = input.MaxPrice,
            OnlyActive = input.OnlyActive,
            Offset = (input.Page - 1) * input.PageSize,
            Limit = input.PageSize
        };
}
