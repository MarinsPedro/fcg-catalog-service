namespace FiapCloudGames.Catalog.Database.IntegrationTests;

public sealed class EfMigrationConventionTests
{
    [Fact(DisplayName = "Obter migrations - devem ser descobertas")]
    public void Get_Migrations_ShouldBeDiscoverable()
    {
        using var context = CatalogDbContextTestFactory.Create();

        Assert.NotEmpty(context.Database.GetMigrations());
    }

    [Fact(DisplayName = "Obter modelo - não deve ter alterações pendentes")]
    public void Get_Model_ShouldHaveNoPendingChanges()
    {
        using var context = CatalogDbContextTestFactory.Create();

        Assert.False(
            context.Database.HasPendingModelChanges(),
            "O modelo do Catalog diverge do snapshot de migrations.");
    }

    [Fact(DisplayName = "Obter DbContext - deve usar o assembly do projeto de migrations")]
    public void Get_DbContext_ShouldUseMigrationsProjectAssembly()
    {
        using var context = CatalogDbContextTestFactory.Create();
        var migrationsAssembly = context.GetService<IMigrationsAssembly>();

        Assert.Equal(typeof(CatalogDbContextFactory).Assembly, migrationsAssembly.Assembly);
    }
}
