using Hangfire;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;

public interface IFearAndGreedJob
{
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task GetFearAndGreedAsync();
}