using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Application.BackgroundJobs;

public class CreateHangfireJobs(IRecurringJobManager jobManager) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jobManager.AddOrUpdate<IFearAndGreedJob>(JobsIdentifier.FearAndGreedJob, job => job.GetFearAndGreedAsync(), "0 0 * * *");
        
        jobManager.AddOrUpdate<ICoinGeckoJobs>(JobsIdentifier.CoinGeckoInitJob, job => job.InitCoinsAsync(), "0 0 5 31 2 ?"); 
        
        jobManager.AddOrUpdate<IIndicatorsJobs>(JobsIdentifier.IndicatorsJob, job => job.CalculateIndicatorsAsync(), "0 0 5 31 2 ?");
        
        //"0 0 5 31 2 ?" - Never fires
        //CoinGeckoInitJob>CoinGeckoTracerJob>IndicatorsJob
        
        //Jobs triggered at startup:
        jobManager.Trigger(JobsIdentifier.FearAndGreedJob);
        jobManager.Trigger(JobsIdentifier.CoinGeckoInitJob); 
        
        return Task.CompletedTask;
    }
}