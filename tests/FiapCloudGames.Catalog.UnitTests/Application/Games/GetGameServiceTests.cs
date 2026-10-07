namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class GetGameServiceTests
{
    [Fact(DisplayName = "Obter jogo - deve retornar o jogo com o ID especificado")]
    public async Task ExecuteAsync_WithGuid_ReturnsGame()
    {
        // Arrange
        var game = Game.Create(
            "Test title Game",
            "Test game description",
            "Test category game",
            0.0m,
            CatalogTestData.NowUtc);
        var fakeRepository = new FakeGameRepository { Game = game };
        var service = new GetGameService(fakeRepository, NullLogger<GetGameService>.Instance);

        // Act
        var result = await service.ExecuteAsync(fakeRepository.Game.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact(DisplayName = "Obter jogo - deve lançar exceção ao não encontrar o jogo")]
    public async Task ExecuteAsync_WithGuid_ReturnAppException()
    {
        // Arrange
        var game = Game.Create(
            "Test title Game",
            "Test game description",
            "Test category game",
            0.0m,
            CatalogTestData.NowUtc);
        var fakeRepository = new FakeGameRepository { Game = game };
        var service = new GetGameService(fakeRepository, NullLogger<GetGameService>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(async () =>
        {
            await service.ExecuteAsync(Guid.NewGuid(), CancellationToken.None);
        });
    }
}
