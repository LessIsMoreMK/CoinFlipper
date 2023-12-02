using CoinFlipper.ServiceDefaults.Options;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<IFearAndGreedIndexClient, FearAndGreedIndexClient>()
            .AddScoped<IFearAndGreedRepository, FearAndGreedRepository>()
            .AddSingleton<IFearAndGreedJob, FearAndGreedJob>()
            
            .AddHangfireServer()
            .AddHostedService<CreateHangfireJobs>();

        return services;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var postgresOptions = builder.Services.GetOptions<PostgresOptions>("Postgres");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgresOptions.ConnectionString));

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