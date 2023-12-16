using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Indicators;
using CoinFlipper.Tracer.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class IndicatorsJobs(
        ILogger<IndicatorsJobs> logger,
        IMovingAverageIndicatorService _movingAverageIndicatorService
        ): IIndicatorsJobs
{
    #region Methods
    
    //TODO: Configuration
    private readonly List<Coin> Coins = new()
    {
        new Coin(new Guid("176de950-d825-4a94-95cc-311c567b92e0"), "Bitcoin", "BTC", "bitcoin"), 
        new Coin(new Guid("c7c47ea0-8f9f-451d-8373-f0e1e6b1d651"), "Ethereum", "ETH", "ethereum"), 
        // new Coin(new Guid("4eb4bdc3-df4b-4917-8684-99f51094b982"), "Binance Coin", "BNB", "binancecoin"), 
        // new Coin(new Guid("2a5b1ff2-4c03-41d0-8fc9-179498b9b264"), "Solana", "SOL", "solana"), 
        // new Coin(new Guid("aa0b5a9a-63b4-4e4b-86f1-44a2362ace59"), "Polygon", "MATIC", "matic-network"), 
        // new Coin(new Guid("a56c2efb-c982-49cc-aa9f-d93896119e10"), "Injective", "INJ", "injective-protocol") 
    };
    
    public async Task CalculateIndicatorsAsync()
    {
        await MovingAverages();
    }
    
    #endregion
    
    #region Private Helpers
    
    private async Task MovingAverages()
    {
        var periods = new List<int>() {21, 50, 100, 200};
        var movingAverages = new List<MovingAverage> {MovingAverage.EMA, MovingAverage.SMA};
        
        foreach (var coin in Coins)
            foreach (var period in periods)
                foreach (var movingAverage in movingAverages)
                {
                    try
                    {
                        _ = movingAverage switch
                        {
                            MovingAverage.SMA => await _movingAverageIndicatorService.CalculateSMA(period, coin.Id, coin.Symbol),
                            MovingAverage.EMA => await _movingAverageIndicatorService.CalculateEMA(period, coin.Id, coin.Symbol),
                        };
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error when calculating {Period} {MovingAverage} for {Symbol}", period, movingAverage, coin.Symbol);
                    }
                }
    }
    
    #endregion
}