namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class ActivateGameServiceTests
{
    [Fact(DisplayName = "Ativar jogo - deve ativar o jogo")]
    public async Task ExecuteAsync_ActivateGame_ReturnsSuccess()
    {
        // Arrange (por padrão o jogo já nasce ativado)
        var game = Game.Create(
            "Test title Game",
            "Test game description",
            "Test category game",
            0.0m,
            CatalogTestData.NowUtc);

        game.Deactivate(); // Desativa o jogo para o teste

        var fakeRepository = new FakeGameRepository { Game = game };
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new ActivateGameService(fakeRepository, unitOfWork, NullLogger<ActivateGameService>.Instance);

        // Act
        await service.ExecuteAsync(game.Id, CancellationToken.None);

        // Assert
        Assert.True(game.IsActive);
    }

    [Fact(DisplayName = "Ativar jogo já ativado - deve falhar ao tentar ativar jogo já ativado.")]
    public async Task ExecuteAsync_ActivateGame_ReturnsAppException()
    {
        // Arrange (por padrão o jogo já nasce ativado)
        var game = Game.Create(
            "Test title Game",
            "Test game description",
            "Test category game",
            0.0m,
            CatalogTestData.NowUtc);

        var fakeRepository = new FakeGameRepository { Game = game };
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new ActivateGameService(fakeRepository, unitOfWork, NullLogger<ActivateGameService>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(() =>
        {
            return service.ExecuteAsync(game.Id, CancellationToken.None);
        });
    }
}
