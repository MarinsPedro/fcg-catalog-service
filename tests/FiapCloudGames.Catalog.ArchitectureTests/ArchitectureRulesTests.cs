namespace FiapCloudGames.Catalog.ArchitectureTests;

public class ArchitectureRulesTests
{
    [Fact(DisplayName = "Verificar referências de projeto - devem seguir a direção da arquitetura limpa")]
    public void Check_ProjectReferences_FollowCleanArchitectureDirection()
    {
        AssertProjectReferences("Domain");
        AssertProjectReferences("Application", "Domain");
        AssertProjectReferences("Infrastructure", "Application", "Domain");
        AssertProjectReferences("Presentation", "Application");
        AssertProjectReferences("Api", "Infrastructure", "Presentation");
        AssertProjectReferences("Migrations", "Infrastructure");
    }

    [Fact(DisplayName = "Verificar referências de teste unitário - devem referenciar apenas Domain e Application")]
    public void Check_UnitTests_ReferenceOnlyDomainAndApplication()
    {
        var root = FindRepositoryRoot();
        var projectPath = Path.Combine(
            root,
            "tests",
            "FiapCloudGames.Catalog.UnitTests",
            "FiapCloudGames.Catalog.UnitTests.csproj");
        var project = XDocument.Load(projectPath);
        var projectReferences = project
            .Descendants("ProjectReference")
            .Select(element => Path.GetFileNameWithoutExtension(
                (string?)element.Attribute("Include")))
            .OfType<string>()
            .Order(StringComparer.Ordinal)
            .ToArray();
        var packageReferences = project
            .Descendants("PackageReference")
            .Select(element => (string?)element.Attribute("Include") ?? string.Empty)
            .ToArray();

        Assert.Equal(
            [
                "FiapCloudGames.Catalog.Application",
                "FiapCloudGames.Catalog.Domain"
            ],
            projectReferences);
        Assert.DoesNotContain(
            packageReferences,
            package => package.Contains(
                "EntityFrameworkCore",
                StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            packageReferences,
            package => package.Contains(
                "Mvc.Testing",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Verificar referências do domínio - não devem referenciar camadas externas ou frameworks")]
    public void Check_Domain_DoesNotReferenceOuterLayersOrFrameworks()
    {
        AssertDoesNotReference(
            typeof(Game).Assembly,
            "Application",
            "Infrastructure",
            "Presentation",
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore");
    }

    [Fact(DisplayName = "Verificar referências da aplicação - não devem referenciar camadas externas ou frameworks HTTP e de dados")]
    public void Check_Application_DoesNotReferenceOuterLayersOrHttpAndDataFrameworks()
    {
        AssertDoesNotReference(
            typeof(FiapCloudGames.Catalog.Application.DependencyInjection).Assembly,
            "Infrastructure",
            "Presentation",
            "Microsoft.AspNetCore",
            "Microsoft.EntityFrameworkCore");
    }

    [Fact(DisplayName = "Verificar referências das migrations do Entity Framework - devem estar apenas no projeto de migrations")]
    public void Check_EfMigrations_LiveOnlyInMigrationsExecutable()
    {
        var root = FindRepositoryRoot();
        var infrastructureMigrations = Path.Combine(
            root,
            "src",
            "FiapCloudGames.Catalog.Infrastructure",
            "Persistence",
            "Migrations");
        var migrationsDirectory = Path.Combine(
            root,
            "src",
            "FiapCloudGames.Catalog.Migrations",
            "Migrations");
        var migrationsProjectFile = Path.Combine(
            root,
            "src",
            "FiapCloudGames.Catalog.Migrations",
            "FiapCloudGames.Catalog.Migrations.csproj");
        var migrationsAppSettings = Path.Combine(
            root,
            "src",
            "FiapCloudGames.Catalog.Migrations",
            "appsettings.json");

        Assert.False(
            Directory.Exists(infrastructureMigrations) &&
            Directory.GetFiles(
                infrastructureMigrations,
                "*.cs",
                SearchOption.AllDirectories).Length > 0);
        Assert.True(Directory.Exists(migrationsDirectory));
        Assert.NotEmpty(Directory.GetFiles(
            migrationsDirectory,
            "*.cs",
            SearchOption.TopDirectoryOnly));
        Assert.True(File.Exists(migrationsAppSettings));

        var migrationsProject = XDocument.Load(migrationsProjectFile);
        Assert.Equal(
            "Exe",
            migrationsProject.Descendants("OutputType")
                .Single()
                .Value);
        Assert.Equal(
            "PreserveNewest",
            migrationsProject
                .Descendants("None")
                .Single(element =>
                    element.Attribute("Update")?.Value == "appsettings*.json")
                .Element("CopyToOutputDirectory")
                ?.Value);
    }

    private static void AssertDoesNotReference(Assembly assembly, params string[] forbiddenParts)
    {
        var references = assembly.GetReferencedAssemblies().Select(reference => reference.Name ?? string.Empty);
        var violations = references
            .Where(reference => forbiddenParts.Any(
                part => reference.Contains(part, StringComparison.Ordinal)))
            .ToArray();

        Assert.Empty(violations);
    }

    private static void AssertProjectReferences(string layer, params string[] expectedLayers)
    {
        var root = FindRepositoryRoot();
        var projectPath = Path.Combine(
            root,
            "src",
            $"FiapCloudGames.Catalog.{layer}",
            $"FiapCloudGames.Catalog.{layer}.csproj");
        var references = XDocument.Load(projectPath)
            .Descendants("ProjectReference")
            .Select(element => Path.GetFileNameWithoutExtension((string?)element.Attribute("Include")))
            .Select(name => name?.Split('.').Last())
            .Where(name => name is not null)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(expectedLayers.Order(StringComparer.Ordinal), references);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "FiapCloudGames.Catalog.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("A raiz do repositório não foi encontrada.");
    }
}
