using Hangfire;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;

public interface IIndicatorsJobs
{
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task CalculateIndicatorsAsync();
}