namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class ListGameServiceTests
{
    [Fact(DisplayName = "Listar jogos - deve retornar uma lista de jogos")]
    public async Task ExecuteAsync_ReturnsListOfGames()
    {
        // Arrange
        var game1 = Game.Create(
            "Test title Game 1",
            "Test game description 1",
            "Test category game 1",
            0.0m,
            CatalogTestData.NowUtc);
        var game2 = Game.Create(
            "Test title Game 2",
            "Test game description 2",
            "Test category game 2",
            10.0m,
            CatalogTestData.NowUtc);
        var fakeRepository = new FakeGameRepository { Games = new List<Game> { game1, game2 } };
        var service = new ListGameService(fakeRepository, NullLogger<ListGameService>.Instance);
        var input = new GameQueryInput() { NumberPage = 1, PageSize = 10 };

        // Act
        var result = await service.ExecuteAsync(input, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItems);
    }
}
