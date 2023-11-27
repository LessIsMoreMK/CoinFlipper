using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Clients;
using CoinFlipper.Tracer.Infrastructure.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFearAndGreedIndexClient, FearAndGreedIndexClient>();
        services.AddScoped<IFearAndGreedRepository, FearAndGreedRepository>();

        return services;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("db");
        
        return builder;
    }
}