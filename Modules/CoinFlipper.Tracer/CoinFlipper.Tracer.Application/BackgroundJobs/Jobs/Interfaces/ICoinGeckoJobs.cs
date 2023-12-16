using Hangfire;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;

public interface ICoinGeckoJobs
{
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task InitCoinsAsync();
    
    [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 3)]
    Task TrackCoinsAsync();
}