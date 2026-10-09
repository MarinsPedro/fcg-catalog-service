namespace FiapCloudGames.Catalog.Migrations.DesignTime;

public sealed class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var builder = MigrationConnection.CreateBuilder(args);
        var options = new DbContextOptionsBuilder<CatalogDbContext>();
        MigrationDbContextOptions.Configure(
            options,
            MigrationConnection.GetRequired(builder.Configuration));

        return new CatalogDbContext(options.Options);
    }
}
