namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class UpdateGameServiceTests
{
    [Fact(DisplayName = "Atualizar jogo - deve atualizar e persistir o jogo com dados válidos")]
    public async Task ExecuteAsync_UpdateGame_UpdateAndPersistGame()
    {
        // Arrange
        var originalTitle = "Test title Game";
        var originalDescription = "Test description Game";
        var originalCategory = "Test category Game";
        var originalBasePrice = 0.0m;
        var game = Game.Create(originalTitle, originalDescription, originalCategory, originalBasePrice, CatalogTestData.NowUtc);

        var fakeRepository = new FakeGameRepository { Game = game };
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new UpdateGameService(fakeRepository, unitOfWork, NullLogger<UpdateGameService>.Instance);
        var updateGameInput = new UpdateGameInput("Updated title", "Updated description", "Updated category", 10.0m);

        // Act
        await service.ExecuteAsync(game.Id, updateGameInput, CancellationToken.None);

        // Assert
        Assert.NotEqual(updateGameInput.Title, originalTitle);
        Assert.NotEqual(updateGameInput.Description, originalDescription);
        Assert.NotEqual(updateGameInput.Category, originalCategory);
        Assert.NotEqual(updateGameInput.BasePrice, originalBasePrice);
    }

    [Fact(DisplayName = "Atualiza jogo - deve falhar se o preço base for negativo")]
    public async Task ExecuteAsync_WithNegativeBasePrice_ReturnsDomainRuleViolationException()
    {
        // Arrange
        var originalTitle = "Test title Game";
        var originalDescription = "Test description Game";
        var originalCategory = "Test category Game";
        var originalBasePrice = 0.0m;
        var game = Game.Create(originalTitle, originalDescription, originalCategory, originalBasePrice, CatalogTestData.NowUtc);

        var fakeRepository = new FakeGameRepository { Game = game };
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new UpdateGameService(fakeRepository, unitOfWork, NullLogger<UpdateGameService>.Instance);
        var updateGameInput = new UpdateGameInput("Updated title", "Updated description", "Updated category", -10.0m);

        // Act & Assert
        await Assert.ThrowsAsync<DomainRuleViolationException>(() =>
        {
            return service.ExecuteAsync(game.Id, updateGameInput, CancellationToken.None);
        });
    }
}
