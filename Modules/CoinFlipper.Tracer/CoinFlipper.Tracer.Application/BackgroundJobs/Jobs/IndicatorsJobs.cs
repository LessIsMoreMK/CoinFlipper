using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using CoinFlipper.Tracer.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class IndicatorsJobs(
        ILogger<IndicatorsJobs> logger,
        IMovingAverageIndicatorService movingAverageIndicatorService,
        IRedisCacheService redisCacheService
        ): IIndicatorsJobs
{
    #region Methods
    
    private IReadOnlyCollection<Coin> Coins = null!;

    public async Task CalculateIndicatorsAsync()
    {
        Coins = redisCacheService.GetCoins();
        
        await MovingAverages();
    }
    
    #endregion
    
    #region Private Methods
    
    private async Task MovingAverages()
    {
        var lengths = new List<int>() {21, 50, 100, 200};
        var movingAverages = new List<MovingAverage>
        {
            MovingAverage.SMA, MovingAverage.EMA,  MovingAverage.VWAP,
            MovingAverage.SMMA, MovingAverage.WMA, MovingAverage.HMA
        };
        
        foreach (var coin in Coins)
            foreach (var length in lengths)
                foreach (var movingAverage in movingAverages)
                {
                    try
                    {
                        _ = movingAverage switch
                        {
                            MovingAverage.SMA => await movingAverageIndicatorService.CalculateSMA(length, coin.Id, coin.Symbol),
                            MovingAverage.EMA => await movingAverageIndicatorService.CalculateEMA(length, coin.Id, coin.Symbol),
                            MovingAverage.VWAP => await movingAverageIndicatorService.CalculateVWAP(length, coin.Id, coin.Symbol),
                            MovingAverage.SMMA => await movingAverageIndicatorService.CalculateSMMA(length, coin.Id, coin.Symbol),
                            MovingAverage.WMA => await movingAverageIndicatorService.CalculateWMA(length, coin.Id, coin.Symbol),
                            MovingAverage.HMA => await movingAverageIndicatorService.CalculateHMA(length, coin.Id, coin.Symbol),
                            _ => throw new ArgumentOutOfRangeException(nameof(movingAverage))
                        };
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error occured while calculating {Length} {MovingAverage} for {Symbol}", length, movingAverage, coin.Symbol);
                    }
                }
    }
    
    #endregion
}