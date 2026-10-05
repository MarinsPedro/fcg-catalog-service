namespace FiapCloudGames.Catalog.Domain.Common;

public sealed class DomainRuleViolationException(string message) : Exception(message);
