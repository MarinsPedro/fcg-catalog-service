namespace FiapCloudGames.Catalog.UnitTests.Domain.Games;

public class GamePriceTests
{
    [Theory(DisplayName = "Preço do jogo - deve ser válido quando o preço for positivo")]
    [InlineData(0.00)]          // Cenário 1: valor mínimo permitido: 0.00.
    [InlineData(9999999999.99)] // Cenário 2: valor máximo permitido: 9999999999.99.
    public void Create_ShouldBeValid_ReturnGamePriceOk(decimal validPrice)
    {
        // Act
        var gamePrice = GamePrice.Create(validPrice);

        // Assert
        Assert.Equal(validPrice, gamePrice.Amount);
    }

    [Fact(DisplayName = "Preço do jogo - deve ser inválido quando o preço for negativo")]
    public void Create_NegativeNumber_ReturnsDomainRuleViolationException()
    {
        // Arrange
        decimal invalidPrice = -10.00m;

        // Act & Assert
        Assert.Throws<DomainRuleViolationException>(() =>
        {
            GamePrice.Create(invalidPrice);
        });
    }

    [Fact(DisplayName = "Preço do jogo - deve ser inválido quando o preço for o valor máximo de um decimal no .NET")]
    public void Create_DecimalMaxValue_ReturnsDomainRuleViolationException()
    {
        // Arrange
        decimal invalidPrice = decimal.MaxValue;

        // Act & Assert
        Assert.Throws<DomainRuleViolationException>(() =>
        {
            GamePrice.Create(invalidPrice);
        });
    }
}
