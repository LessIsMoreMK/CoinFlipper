using CoinFlipper.ServiceDefaults.Options;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Clients;
using CoinFlipper.Tracer.Infrastructure.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.EntityFrameworkCore;
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
        var postgresOptions = builder.Services.GetOptions<PostgresOptions>("Postgres");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgresOptions.ConnectionString));
        
        return builder;
    }
}