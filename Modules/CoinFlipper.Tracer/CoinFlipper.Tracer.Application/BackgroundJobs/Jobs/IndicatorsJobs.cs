using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using CoinFlipper.Tracer.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class IndicatorsJobs(
        ILogger<IndicatorsJobs> logger,
        IMovingAverageIndicatorService _movingAverageIndicatorService,
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
        var periods = new List<int>() {21, 50, 100, 200};
        var movingAverages = new List<MovingAverage>
        {
            MovingAverage.SMA, MovingAverage.EMA,  MovingAverage.VWAP,
            MovingAverage.SMMA, MovingAverage.WMA, MovingAverage.HMA
        };
        
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
                            MovingAverage.VWAP => await _movingAverageIndicatorService.CalculateVWAP(period, coin.Id, coin.Symbol),
                            MovingAverage.SMMA => await _movingAverageIndicatorService.CalculateSMMA(period, coin.Id, coin.Symbol),
                            MovingAverage.WMA => await _movingAverageIndicatorService.CalculateWMA(period, coin.Id, coin.Symbol),
                            MovingAverage.HMA => await _movingAverageIndicatorService.CalculateHMA(period, coin.Id, coin.Symbol),
                            _ => throw new ArgumentOutOfRangeException(nameof(movingAverage))
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