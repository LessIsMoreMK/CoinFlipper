using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.Services.Indicators;

public class MovingAverageIndicatorService(
    ILogger<MovingAverageIndicatorService> logger,
    IRedisCacheService redisCacheService
    ) : IMovingAverageIndicatorService
{
    #region Methods
    
    public async Task<decimal?> CalculateSMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period);

        if (!Validate(coinDataRecords, period, coinSymbol, "SMA"))
            return null;

        var sma = coinDataRecords.Average(record => record.Price);
        
        logger.LogInformation("#INFO {Symbol} {Period} SMA: {SMA}", coinSymbol, period, sma);
        
        return sma;
    }

    public async Task<decimal?> CalculateEMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period);
        
        if (!Validate(coinDataRecords, period, coinSymbol, "EMA"))
            return null;
        
        var alpha = 2 / (decimal)(period + 1);
        var prices = coinDataRecords.Select(m => m.Price).ToArray();
        var ema = prices[0];
        
        for (var i = 0; i < prices.Length; i++)
            ema = i == 0
                ? prices[i]
                : alpha*prices[i] + (1 - alpha) * ema;
        
        logger.LogInformation("#INFO {Symbol} {Period} EMA: {EMA}", coinSymbol, period, ema);
        
        return ema;
    }
    
    public async Task<decimal?> CalculateVWAP(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period);

        if (!Validate(coinDataRecords, period, coinSymbol, "VWAP"))
            return null;

        decimal periodVolume = 0;
        decimal cumulativeVWAP = 0;

        foreach (var coinData in coinDataRecords)
        {
            periodVolume += coinData.Volume;
            cumulativeVWAP += coinData.Price * coinData.Volume;
        }
        
        var vwap = cumulativeVWAP / periodVolume;
        
        logger.LogInformation("#INFO {Symbol} {Period} VWAP: {VWAP}", coinSymbol, period, vwap);

        return vwap;
    }
    
    public async Task<decimal?> CalculateSMMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period*2);

        if (!Validate(coinDataRecords, period*2, coinSymbol, "SMMA"))
            return null;

        // Initial SMMA is a SMA from previous period records
        decimal smma = coinDataRecords.Skip(period).Average(c => c.Price); 
    
        foreach (var coinData in coinDataRecords.Take(period))
            smma = ((smma * (period - 1)) + coinData.Price) / period;
        
        logger.LogInformation("#INFO {Symbol} {Period} SMMA: {SMMA}", coinSymbol, period, smma);

        return smma;
    }
    
    public async Task<decimal?> CalculateWMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period);

        if (!Validate(coinDataRecords, period, coinSymbol, "WMA"))
            return null;

        decimal currentWeight = period;
        decimal weighting = (decimal)period * (period + 1) / 2;
        decimal wma = 0;
        
        foreach (var coinData in coinDataRecords)
        {
            wma += coinData.Price * (currentWeight / weighting);
            currentWeight--;
        }
        
        logger.LogInformation("#INFO {Symbol} {Period} WMA: {WMA}", coinSymbol, period, wma);

        return wma;
    }

    public async Task<decimal?> CalculateHMA(int period, Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCachedCoinDataListAsync(coinId, period);

        if (!Validate(coinDataRecords, period, coinSymbol, "HMA"))
            return null;

        var halfPeriod = (int)Math.Round((double)period / 2);
        var halfPeriodWMA = CalculateWMA(halfPeriod, coinDataRecords.Take(halfPeriod).ToList());
        var fullPeriodWMA = CalculateWMA(period, coinDataRecords);

        var rawHma = (2 * halfPeriodWMA) - fullPeriodWMA;
        var sqrtPeriod = (int)Math.Sqrt(period);
        
        var hma = CalculateAdaptiveWeightHMA((double)rawHma!.Value, sqrtPeriod, coinDataRecords.Take(sqrtPeriod).ToList());
        
        logger.LogInformation("#INFO {Symbol} {Period} HMA: {HMA}", coinSymbol, period, hma);

        return (decimal)hma;
    }
    
    #endregion
    
    #region Private Helpers
    
    private static decimal? CalculateWMA(int period, List<CoinData> coinDataRecords)
    {
        decimal currentWeight = period;
        decimal weighting = (decimal)period * (period + 1) / 2;
        decimal wma = 0;
        
        foreach (var coinData in coinDataRecords)
        {
            wma += coinData.Price * (currentWeight / weighting);
            currentWeight--;
        }
        
        return wma;
    }
    
    //TODO: !(double)
    private double CalculateAdaptiveWeightHMA(double rawHma, int sqrtPeriod, List<CoinData> coinDataRecords)
    {
        var weightingFactors = new List<double>();
        foreach (var coinData in coinDataRecords)
        {
            var weightingFactor = (2 * Math.Pow(2, sqrtPeriod)) / (2 * Math.Pow((double) coinData.Price, sqrtPeriod));
            weightingFactors.Add(weightingFactor);
        }

        double hma = 0;
        for (var i = 0; i < coinDataRecords.Count; i++)
        {
            if (weightingFactors[i] == 0)
                continue; // Skip data points with zero weighting factor

            hma += rawHma * weightingFactors[i];
        }
        
        return hma / weightingFactors.Sum();
    }
    
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
            logger.LogError("Database does not contain enough {Symbol} prices. " +
                            "Cannot calculate {AverageName} for expected period: {Period}; actual records {RecordsCount}",
                coinSymbol, averageName, period, coinDataRecords.Count);
            return false;
        }
        
        //TODO: Improve 
        var lastDate = DateTime.UtcNow.AddMinutes(-period * 5 - 8); //This setting allows for missing one record in this currently working 5 minutes interval
        if (coinDataRecords.Last().DateTime > lastDate)
            return true;
        
        logger.LogError("Database does not contain valid {Symbol} prices. " +
                        "Cannot calculate {MovingAverage} for period: {Period}",
            coinSymbol, averageName, period);
        return false;
    }
    
    #endregion
}