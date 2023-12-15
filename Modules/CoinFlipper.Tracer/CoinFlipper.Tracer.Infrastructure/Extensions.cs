using CoinFlipper.ServiceDefaults;
using CoinFlipper.Tracer.Application.BackgroundJobs;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Clients;
using CoinFlipper.Tracer.Infrastructure.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Hangfire;
using Hangfire.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Infrastructure;

public static class Extensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddPostgresDatabase<ApplicationDbContext>();
        
        builder.Services
            .AddScoped<IFearAndGreedIndexClient, FearAndGreedIndexClient>()
            .AddScoped<ICoinGeckoClient, CoinGeckoClient>()
            
            .AddScoped<IFearAndGreedRepository, FearAndGreedRepository>()
            .AddScoped<ICoinRepository, CoinRepository>()
            .AddScoped<ICoinDataRepository, CoinDataRepository>()
            
            .AddSingleton<IFearAndGreedJob, FearAndGreedJob>()
            .AddSingleton<ICoinGeckoTracerJob, CoinGeckoTracerJob>()
            .AddHangfireServer()
            .AddHostedService<CreateHangfireJobs>();

        return builder;
    }
    
    private static IServiceCollection AddHangfireServer(this IServiceCollection services)
    {
        services.AddHangfireServer(config =>
        {
            config.WorkerCount = 1;
            config.SchedulePollingInterval = TimeSpan.FromSeconds(10);
            config.Queues = new[]
            {
                "default",
                "recurring"
            };
        });

        services.AddHangfire(config =>
        {
            config.UseInMemoryStorage(new InMemoryStorageOptions() { DisableJobSerialization = true });
        });

        return services;
    }
}