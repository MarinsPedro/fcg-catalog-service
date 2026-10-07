namespace FiapCloudGames.Catalog.UnitTests.Domain.Games;

public class GameTests
{
    [Fact(DisplayName = "Instancia jogo - deve instanciar jogo com dados válidos")]
    public void Create_GameWithValidData_ReturnsGameInstance()
    {
        // Arrange
        var title = "Test Game";
        var description = "This is a test game.";
        var basePrice = 152.67m;
        var createAt = CatalogTestData.NowUtc;
        var category = "2D fight";

        // Act
        var game = Game.Create(title, description, category, basePrice, createAt);

        // Assert
        Assert.NotNull(game);
        Assert.IsType<Guid>(game.Id);
        Assert.Equal(title, game.Title);
        Assert.Equal(description, game.Description);
        Assert.Equal(basePrice, game.BasePrice.Amount);
        Assert.Equal(category, game.Category);
    }

    [Fact(DisplayName = "Instancia jogo - deve instanciar jogo com dados a serem limpos")]
    public void Create_GameWithDataToBeCleaned_ReturnsGameInstance()
    {
        // Arrange
        var title = "   Test Game   ";
        var description = "   This is a test game.   ";
        var basePrice = 152.67m;
        var createAt = CatalogTestData.NowUtc;
        var category = "   2D fight   ";

        // Act
        var game = Game.Create(title, description, category, basePrice, createAt);

        // Assert
        Assert.NotNull(game);
    }

    [Fact(DisplayName = "Ativa jogo - deve ativar um jogo")]
    public void Activate_Game_ReturnsActiveGame()
    {
        // Arrange
        var title = "Test Game";
        var description = "This is a test game.";
        var basePrice = 152.67m;
        var createAt = CatalogTestData.NowUtc;
        var category = "2D fight";

        // Act
        var game = Game.Create(title, description, category, basePrice, createAt);
        game.Deactivate();
        game.Activate();

        // Assert
        Assert.True(game.IsActive);
    }

    [Fact(DisplayName = "Desativa jogo - deve inativar um jogo")]
    public void Deactivate_Game_ReturnsInactiveGame()
    {
        // Arrange
        var title = "Test Game";
        var description = "This is a test game.";
        var basePrice = 152.67m;
        var createAt = CatalogTestData.NowUtc;
        var category = "2D fight";

        // Act
        var game = Game.Create(title, description, category, basePrice, createAt);
        game.Activate();

        // Assert
        Assert.True(game.IsActive);
    }
}
