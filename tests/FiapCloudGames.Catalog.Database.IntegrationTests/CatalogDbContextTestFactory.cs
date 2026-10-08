namespace FiapCloudGames.Catalog.Database.IntegrationTests;

internal static class CatalogDbContextTestFactory
{
    private const string ConnectionString =
        "Host=localhost;Database=model_only;Username=model;Password=model";

    public static CatalogDbContext Create()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>();
        MigrationDbContextOptions.Configure(options, ConnectionString);
        return new CatalogDbContext(options.Options);
    }
}
