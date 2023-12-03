using CoinFlipper.SwissArmy.Infrastructure.Repositories.Models;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.Configurations;

public class SentenceConfiguration : IEntityTypeConfiguration<SentenceDb>
{
    public void Configure(EntityTypeBuilder<SentenceDb> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasData(Seed.Sentences());
    }
}