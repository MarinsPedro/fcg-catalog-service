using FiapCloudGames.Catalog.Domain.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

namespace FiapCloudGames.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        var priceConverter =
            new ValueConverter<GamePrice, decimal>(
                price => price.Amount,
                value => GamePrice.Create(value));

        builder.ToTable("games");
        builder.HasKey(game => game.Id);
        builder.Property(game => game.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property(game => game.Title).HasColumnName("title").HasMaxLength(160).IsRequired();
        builder.Property(game => game.Description).HasColumnName("description").HasMaxLength(4000).IsRequired();
        builder.Property(game => game.Category).HasColumnName("category").HasMaxLength(80).IsRequired();
        builder.Property(game => game.BasePrice)
            .HasColumnName("base_price")
            .HasConversion(priceConverter)
            .HasPrecision(12, 2)
            .IsRequired();
        builder.Property(game => game.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(game => game.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.HasIndex(game => game.Title);
    }
}
