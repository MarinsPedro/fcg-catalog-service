using FiapCloudGames.Catalog.Domain.Common;

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

        return new GamePrice(decimal.Round(amount, 2, MidpointRounding.ToEven));
    }

    public override string ToString() => Amount.ToString("0.00");
}
