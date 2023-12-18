﻿using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
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
        jobManager.AddOrUpdate<IFearAndGreedJob>(JobsIdentifier.FearAndGreedJob, job => job.GetFearAndGreedAsync(), "0 0 * * *");
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
    public async Task ExecuteChainedJobsAsync()
    {
        await coinGeckoJobs.TrackCoinsAsync();
        await indicatorsJobs.CalculateIndicatorsAsync();
    }
}