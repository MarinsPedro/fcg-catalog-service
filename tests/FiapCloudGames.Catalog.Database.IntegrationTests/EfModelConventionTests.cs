namespace FiapCloudGames.Catalog.Database.IntegrationTests;

public sealed class EfModelConventionTests
{
    [Fact(DisplayName = "Verificar entidades - deve usar esquema de identidade e nomenclatura lower_snake_case")]
    public void Check_Entities_ShouldUseIdentitySchemaAndLowerSnakeCase()
    {
        using var context = CatalogDbContextTestFactory.Create();
        var entities = context.Model.GetEntityTypes().ToArray();

        Assert.NotEmpty(entities);

        foreach (var entity in entities)
        {
            var table = Assert.IsType<string>(entity.GetTableName());
            Assert.Equal("catalog", entity.GetSchema());
            AssertLowerSnakeCase(table);

            var storeObject = StoreObjectIdentifier.Table(
                table,
                entity.GetSchema());

            foreach (var property in entity.GetProperties())
            {
                var column = Assert.IsType<string>(
                    property.GetColumnName(storeObject));
                AssertLowerSnakeCase(column);
            }
        }
    }

    [Fact(DisplayName = "Verificar entidades - devem ter chaves primárias e pertencer ao domínio de identidade")]
    public void Check_Entities_ShouldHavePrimaryKeysAndBelongToIdentityDomain()
    {
        using var context = CatalogDbContextTestFactory.Create();

        foreach (var entity in context.Model.GetEntityTypes())
        {
            Assert.NotNull(entity.FindPrimaryKey());
            Assert.StartsWith(
                "FiapCloudGames.Catalog.Domain",
                entity.ClrType.Namespace,
                StringComparison.Ordinal);
        }
    }

    [Fact(DisplayName = "Verificar propriedades decimais - devem ter precisão explícita")]
    public void Check_DecimalProperties_ShouldHaveExplicitPrecision()
    {
        using var context = CatalogDbContextTestFactory.Create();
        var decimalProperties = context.Model
            .GetEntityTypes()
            .SelectMany(entity => entity.GetProperties())
            .Where(IsStoredAsDecimal)
            .ToArray();

        foreach (var property in decimalProperties)
        {
            Assert.NotNull(property.GetPrecision());
            Assert.NotNull(property.GetScale());
        }
    }

    private static bool IsStoredAsDecimal(IReadOnlyProperty property)
    {
        var providerType = property.GetValueConverter()?.ProviderClrType ?? property.ClrType;
        return providerType == typeof(decimal);
    }

    private static void AssertLowerSnakeCase(string value)
    {
        Assert.True(
            value.Length > 0 &&
            char.IsLower(value[0]) &&
            value.All(character =>
                char.IsLower(character) ||
                char.IsDigit(character) ||
                character == '_'),
            $"'{value}' não segue lower_snake_case.");
    }
}
