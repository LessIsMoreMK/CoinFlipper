using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Application.BackgroundJobs;

public class CreateHangfireJobs(IRecurringJobManager jobManager) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jobManager.AddOrUpdate<IFearAndGreedJob>(JobsIdentifier.FearAndGreedJob, job => job.GetFearAndGreedAsync(), "0 0 * * *");


        //Jobs triggered at startup:
        jobManager.Trigger(JobsIdentifier.FearAndGreedJob);
        
        return Task.CompletedTask;
    }
}