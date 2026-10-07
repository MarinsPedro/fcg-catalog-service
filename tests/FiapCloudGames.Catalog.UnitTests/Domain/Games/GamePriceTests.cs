namespace FiapCloudGames.Catalog.UnitTests.Domain.Games;

public class GamePriceTests
{
    [Theory(DisplayName = "Preço do jogo - deve ser válido quando o preço for positivo")]
    [InlineData("0.00")] // Cenário 1: valor mínimo permitido (zero)
    [InlineData("79228162514264337593543950335")] // Cenário 2: valor máximo possível (limitado ao decimal.MaxValue do .NET)
    public void GamePrice_ShouldBeValid_ReturnGamePriceOk(string validPrice)
    {
        // Arrage
        var parsedValiddPrice = decimal.Parse(validPrice.ToString());

        // Act
        var gamePrice = GamePrice.Create(parsedValiddPrice);

        // Assert
        Assert.Equal(parsedValiddPrice, gamePrice.Amount);
    }

    [Fact(DisplayName = "Preço do jogo - deve ser inválido quando o preço for negativo")]
    public void GamePrice_ShouldBeInvalid_ReturnsDomainRuleViolationException()
    {
        // Arrange
        decimal invalidPrice = -10.00m;
        // Act & Assert
        Assert.Throws<DomainRuleViolationException>(() =>
        {
            GamePrice.Create(invalidPrice);
        });
    }
}
