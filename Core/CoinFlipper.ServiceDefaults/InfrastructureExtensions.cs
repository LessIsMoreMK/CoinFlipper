using CoinFlipper.ServiceDefaults.Database;
using CoinFlipper.ServiceDefaults.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.ServiceDefaults;

public static class InfrastructureExtensions
{
    public static IHostApplicationBuilder AddPostgresDatabase<TDbContext>(this IHostApplicationBuilder builder) where TDbContext : DbContext
    {
        var postgresOptions = builder.Configuration.GetOptions<PostgresOptions>("Postgres");

        if (!postgresOptions.Enabled)
            return builder;

        builder.Services.AddDbContext<TDbContext>(options =>
            options.UseNpgsql(postgresOptions.ConnectionString));

        if (postgresOptions.MigrateOnStartup)
            builder.Services.AddHostedService<DatabaseMigrator<TDbContext>>();

        return builder;
    }
}