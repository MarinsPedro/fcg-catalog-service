using FiapCloudGames.Catalog.Migrations.DesignTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FiapCloudGames.Catalog.Database.IntegrationTests;

public sealed class EfMigrationConventionTests
{
    [Fact]
    public void Migrations_ShouldBeDiscoverable()
    {
        using var context = CatalogDbContextTestFactory.Create();

        Assert.NotEmpty(context.Database.GetMigrations());
    }

    [Fact]
    public void Model_ShouldHaveNoPendingChanges()
    {
        using var context = CatalogDbContextTestFactory.Create();

        Assert.False(
            context.Database.HasPendingModelChanges(),
            "O modelo do Catalog diverge do snapshot de migrations.");
    }

    [Fact]
    public void DbContext_ShouldUseMigrationsProjectAssembly()
    {
        using var context = CatalogDbContextTestFactory.Create();
        var migrationsAssembly = context.GetService<IMigrationsAssembly>();

        Assert.Equal(typeof(CatalogDbContextFactory).Assembly, migrationsAssembly.Assembly);
    }
}
