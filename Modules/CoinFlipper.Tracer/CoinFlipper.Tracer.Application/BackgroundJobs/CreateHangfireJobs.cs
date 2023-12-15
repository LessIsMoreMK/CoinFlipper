using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Application.BackgroundJobs;

public class CreateHangfireJobs(IRecurringJobManager jobManager) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jobManager.AddOrUpdate<IFearAndGreedJob>(JobsIdentifier.FearAndGreedJob, job => job.GetFearAndGreedAsync(), "0 0 * * *");
        jobManager.AddOrUpdate<ICoinGeckoTracerJob>(JobsIdentifier.CoinGeckoInitJob, job => job.InitCoinsAsync(), "0 0 5 31 2 ?"); //Fires only once on app startup
        
        //This job is run after CoinGeckoInitJob
        //jobManager.AddOrUpdate<ICoinGeckoTracerJob>(JobsIdentifier.CoinGeckoTracerJob, job => job.TrackCoinsAsync(), "*/5 * * * *");


        //Jobs triggered at startup:
        jobManager.Trigger(JobsIdentifier.FearAndGreedJob);
        jobManager.Trigger(JobsIdentifier.CoinGeckoInitJob);
        
        return Task.CompletedTask;
    }
}