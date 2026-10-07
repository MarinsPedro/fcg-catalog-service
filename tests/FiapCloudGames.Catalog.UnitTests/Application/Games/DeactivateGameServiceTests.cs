namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class DeactivateGameServiceTests
{
    [Fact(DisplayName = "Desativar jogo - deve desativar o jogo")]
    public async Task ExecuteAsync_DeactivateGame_ReturnSuccess()
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
        var service = new DeactivateGameService(fakeRepository, unitOfWork, NullLogger<DeactivateGameService>.Instance);

        // Act
        await service.ExecuteAsync(game.Id, CancellationToken.None);

        // Assert
        Assert.False(game.IsActive);
    }

    [Fact(DisplayName = "Desativar jogo já desativado - deve falhar ao tentar desativar jogo já desativado.")]
    public async Task ExecuteAsync_DeactivateGame_ReturnAppException()
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
        var service = new DeactivateGameService(fakeRepository, unitOfWork, NullLogger<DeactivateGameService>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(() =>
        {
            return service.ExecuteAsync(game.Id, CancellationToken.None);
        });
    }
}
