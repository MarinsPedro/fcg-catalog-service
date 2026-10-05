using FiapCloudGames.BuildingBlocks.Authentication;
using FiapCloudGames.Catalog.Application;
using FiapCloudGames.Catalog.Application.Abstractions.Persistence;
using FiapCloudGames.Catalog.Domain.Games;
using FiapCloudGames.Catalog.Infrastructure.Authentication;
using FiapCloudGames.Catalog.Infrastructure.Health;
using FiapCloudGames.Catalog.Infrastructure.Persistence;
using FiapCloudGames.Catalog.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FiapCloudGames.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("A connection string 'Database' não foi configurada.");

        services.AddCatalogApplication();
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Issuer),
                "Jwt:Issuer é obrigatório.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.KeyId),
                "Jwt:KeyId é obrigatório.")
            .Validate(
                options => options.UserTokenLifetime > TimeSpan.Zero &&
                    options.ServiceTokenLifetime > TimeSpan.Zero &&
                    options.RefreshTokenLifetime > TimeSpan.Zero,
                "Os tempos de vida dos tokens devem ser maiores que zero.")
            .ValidateOnStart();
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICatalogUnitOfWork>(provider => provider.GetRequiredService<CatalogDbContext>());
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddHealthChecks().AddCheck<CatalogDatabaseHealthCheck>("catalog-database", tags: ["ready"]);

        AddAuthentication(services);
        return services;
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<RsaSigningKeyProvider, Microsoft.Extensions.Options.IOptions<JwtOptions>>(
                (options, provider, jwtOptions) =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Value.Issuer,
                        ValidateAudience = true,
                        ValidAudience = "catalog-api",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = provider.SecurityKey,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        NameClaimType = FiapClaimTypes.Subject,
                        RoleClaimType = FiapClaimTypes.Role
                    };
                });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }
}
