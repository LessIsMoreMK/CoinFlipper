namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;

public interface ICoinGeckoJobs
{
    Task InitCoinsAsync();
    
    Task TrackCoinsAsync();
}