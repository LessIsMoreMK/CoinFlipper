using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.Services.Indicators;

public class MovingAverageIndicatorService(
    ILogger<MovingAverageIndicatorService> logger,
    ICoinDataRepository coinDataRepository
    ) : IMovingAverageIndicatorService
{
    #region Methods
    
    public async Task<decimal?> CalculateSMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await coinDataRepository.GetCoinDataXNewestRecords(coinId, period);

        if (!Validate(coinDataRecords, period, coinSymbol, "SMA"))
            return null;

        var result = coinDataRecords.Average(record => record.Price);
        
        logger.LogInformation("#INFO {Symbol} {Period} SMA: {Result}", coinSymbol, period, result);
        
        return result;
    }

    public async Task<decimal?> CalculateEMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await coinDataRepository.GetCoinDataXNewestRecords(coinId, period);
        
        if (!Validate(coinDataRecords, period, coinSymbol, "EMA"))
            return null;
        
        var alpha = 2 / (decimal)(period + 1);
        var prices = coinDataRecords.Select(m => m.Price).ToArray();
        var result = prices[0];
        
        for (var i = 0; i < prices.Length; i++)
            result = i == 0
                ? prices[i]
                : alpha*prices[i] + (1 - alpha) * result;
        
        logger.LogInformation("#INFO {Symbol} {Period} EMA: {Result}", coinSymbol, period, result);
        
        return result;
    }
    
    #endregion
    
    #region Private Helpers
    
    /// <summary>
    /// Validates if the average is going to be calculated on correct data
    /// </summary>
    /// <param name="coinDataRecords"></param>
    /// <param name="period"></param>
    /// <param name="coinSymbol"></param>
    /// <param name="averageName"></param>
    /// <returns></returns>
    private bool Validate(IReadOnlyCollection<CoinData> coinDataRecords, int period, string coinSymbol, string averageName)
    {
        if (coinDataRecords.Count != period)
        {
            logger.LogError("Database does not contain valid {Symbol} prices. " +
                            "Cannot calculate {AverageName} for expected period: {Period}; actual records {RecordsCount}",
                coinSymbol, averageName, period, coinDataRecords.Count);
            return false;
        }
        
        //TODO: Improve
        var lastDate = DateTime.UtcNow.AddMinutes(-period * 5 + 3);
        if (coinDataRecords.Last().DateTime > lastDate)
            return true;
        
        logger.LogError("Database does not contain valid {Symbol} prices. " +
                        "Cannot calculate {MovingAverage} for period: {Period}",
            coinSymbol, averageName, period);
        return false;
    }
    
    #endregion
}