namespace FiapCloudGames.Catalog.Domain.Games;

public readonly record struct GamePrice
{
    private GamePrice(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount { get; init; }

    public static GamePrice Create(decimal amount)
    {
        if (amount < 0)
            throw new DomainRuleViolationException("O preço base não pode ser negativo.");
        
        if (amount > 9999999999.99m)
            throw new DomainRuleViolationException("O preço base não pode ser maior que 9999999999.99.");

        return new GamePrice(decimal.Round(amount, 2, MidpointRounding.ToEven));
    }

    public override string ToString() => Amount.ToString("0.00");

    #region Implementação de comparativos para traduzir a query para o EF.

    public static bool operator <=(GamePrice left, GamePrice right)
        => left.Amount <= right.Amount;

    public static bool operator >=(GamePrice left, GamePrice right)
        => left.Amount >= right.Amount;

    public static bool operator <(GamePrice left, GamePrice right)
        => left.Amount < right.Amount;

    public static bool operator >(GamePrice left, GamePrice right)
        => left.Amount > right.Amount;

    #endregion
}
