using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Configurations;

public class FearAndGreedConfiguration : IEntityTypeConfiguration<FearAndGreedDb>
{
    private static readonly DateTimeKindValueConverter DateTimeKindValueConverter = new(DateTimeKind.Utc);
    
    public void Configure(EntityTypeBuilder<FearAndGreedDb> builder)
    {
        builder.HasKey(a => a.DateTime);
        builder.HasIndex(a => a.DateTime)
            .IsDescending(false);

        builder.Property(a => a.DateTime)
            .IsRequired()
            .HasConversion(DateTimeKindValueConverter);

        builder.Property(a => a.Value)
            .IsRequired()
            .HasPrecision(0);
    }
}