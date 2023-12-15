using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Configurations;

public class CoinDataConfiguration : IEntityTypeConfiguration<CoinDataDb>
{
    private static readonly DateTimeKindValueConverter DateTimeKindValueConverter = new(DateTimeKind.Utc);
    
    public void Configure(EntityTypeBuilder<CoinDataDb> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.DateTime)
            .IsRequired()
            .HasConversion(DateTimeKindValueConverter);
    }
}