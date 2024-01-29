using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Application.BackgroundJobs;

public class CreateHangfireJobs(
    IRecurringJobManager jobManager, 
    ICoinGeckoJobs coinGeckoJobs
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jobManager.AddOrUpdate<IFearAndGreedJob>(JobsIdentifier.FearAndGreedJob, job => job.GetFearAndGreedAsync(), "0 1 * * *");
        jobManager.Trigger(JobsIdentifier.FearAndGreedJob);

        await ExecuteChainedJobsAsync();
    }
    
    private async Task ExecuteChainedJobsAsync()
    {
        await coinGeckoJobs.InitCoinsAsync();
        
        jobManager.AddOrUpdate<JobOrchestrator>(JobsIdentifier.ChainedJobs, job => job.ExecuteChainedJobsAsync(), "*/5 * * * *"); 
    }
}

public class JobOrchestrator(ICoinGeckoJobs coinGeckoJobs, IIndicatorsJobs indicatorsJobs) 
{
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 5, DelaysInSeconds = new [] {20})]
    public async Task ExecuteChainedJobsAsync()
    {
        await coinGeckoJobs.TrackCoinsAsync();
        await indicatorsJobs.CalculateIndicatorsAsync();
    }
}