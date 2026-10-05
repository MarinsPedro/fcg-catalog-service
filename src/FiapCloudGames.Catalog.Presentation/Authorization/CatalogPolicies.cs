namespace FiapCloudGames.Catalog.Presentation.Authorization;

public static class CatalogPolicies
{
    public const string GamesCreate = "catalog.games.create";
    public const string GamesRead = "catalog.games.read";
    public const string GamesWrite = "catalog.games.write";
    public const string ClientsCreate = "catalog.clients.create";
    public const string ClientsRead = "catalog.clients.read";
    public const string ClientsWrite = "catalog.clients.write";
    public const string AuditRead = "catalog.audit.read";
}
