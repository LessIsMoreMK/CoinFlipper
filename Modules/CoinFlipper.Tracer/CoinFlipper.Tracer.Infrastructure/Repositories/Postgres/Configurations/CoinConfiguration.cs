using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Configurations;

public class CoinConfiguration : IEntityTypeConfiguration<CoinDb>
{
    public void Configure(EntityTypeBuilder<CoinDb> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasAlternateKey(a => a.Symbol);
        builder.HasAlternateKey(a => a.CoinGeckoId);

        builder.HasIndex(a => a.Id);
        builder.HasIndex(a => a.Symbol);
        builder.HasIndex(a => a.CoinGeckoId);

        builder.HasMany<CoinDataDb>(a => a.CoinData)
            .WithOne(b => b.Coin)
            .HasForeignKey(b => b.CoinId);
    }
}