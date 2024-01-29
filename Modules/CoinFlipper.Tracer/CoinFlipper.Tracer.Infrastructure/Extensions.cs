using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Options;
using CoinFlipper.Tracer.Application.BackgroundJobs;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.Services.Indicators;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using CoinFlipper.Tracer.Infrastructure.Clients;
using CoinFlipper.Tracer.Infrastructure.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using CoinFlipper.Tracer.Infrastructure.Services;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Infrastructure;

public static class Extensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddPostgresDatabase<ApplicationDbContext>()
            .AddDistributedCache();
        
        builder.Services
            .AddScoped<IFearAndGreedIndexClient, FearAndGreedIndexClient>()
            .AddScoped<ICoinGeckoClient, CoinGeckoClient>()
            
            .AddSingleton<IMovingAverageIndicatorService, MovingAverageIndicatorService>()
            
            .AddSingleton<IRedisCacheService, RedisCacheService>()
            
            .AddScoped<IFearAndGreedRepository, FearAndGreedRepository>()
            .AddScoped<ICoinRepository, CoinRepository>()
            .AddScoped<ICoinDataRepository, CoinDataRepository>()
            
            .AddScoped<IFearAndGreedJob, FearAndGreedJob>()
            .AddScoped<ICoinGeckoJobs, CoinGeckoJobs>()
            .AddScoped<IIndicatorsJobs, IndicatorsJobs>()
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
        
        var redisOptions = services.GetOptions<RedisOptions>("Redis");

        services.AddHangfire(config => 
            config.UseRedisStorage(redisOptions.Address));

        return services;
    }
}