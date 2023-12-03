using CoinFlipper.ServiceDefaults;
using CoinFlipper.SwissArmy.Domain.Repositories;
using CoinFlipper.SwissArmy.Infrastructure.Repositories;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.SwissArmy.Infrastructure;

public static class Extensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddPostgresDatabase<ApplicationDbContext>();
        
        builder.Services
            .AddScoped<ISentenceRepository, SentenceRepository>()
            
            ;

        return builder;
    }
}