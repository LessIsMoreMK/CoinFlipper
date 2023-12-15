using Hangfire;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;

public interface ICoinGeckoTracerJob
{
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task TrackCoinsAsync();

    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task InitCoinsAsync();
}