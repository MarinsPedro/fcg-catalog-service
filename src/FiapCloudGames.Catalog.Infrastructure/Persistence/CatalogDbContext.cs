namespace FiapCloudGames.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options), ICatalogUnitOfWork
{
    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);        
    }
}
