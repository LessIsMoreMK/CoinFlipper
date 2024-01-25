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
    
    public async Task<decimal?> CalculateSMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length);

        if (!Validate(coinDataRecords, length, coinSymbol, "SMA", validateDateTime))
            return null;

        var sma = coinDataRecords.Average(record => record.Price);
        
        logger.LogInformation("#INFO {Symbol} {Length} SMA: {SMA}", coinSymbol, length, sma);
        
        return sma;
    }

    public async Task<decimal?> CalculateEMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length);
        
        if (!Validate(coinDataRecords, length, coinSymbol, "EMA", validateDateTime))
            return null;
        
        var alpha = 2 / (decimal)(length + 1);
        var prices = coinDataRecords.Select(m => m.Price).ToArray();
        var ema = prices[0];
        
        for (var i = 1; i < prices.Length; i++)
            ema = alpha * prices[i] + (1 - alpha) * ema;
        
        logger.LogInformation("#INFO {Symbol} {Length} EMA: {EMA}", coinSymbol, length, ema);
        
        return ema;
    }
    
    public async Task<decimal?> CalculateVWAP(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length);

        if (!Validate(coinDataRecords, length, coinSymbol, "VWAP", validateDateTime))
            return null;

        decimal LengthVolume = 0;
        decimal cumulativeVWAP = 0;

        foreach (var coinData in coinDataRecords)
        {
            LengthVolume += coinData.Volume;
            cumulativeVWAP += coinData.Price * coinData.Volume;
        }
        
        var vwap = cumulativeVWAP / LengthVolume;
        
        logger.LogInformation("#INFO {Symbol} {Length} VWAP: {VWAP}", coinSymbol, length, vwap);

        return vwap;
    }
    
    public async Task<decimal?> CalculateSMMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length*2);

        if (!Validate(coinDataRecords, length*2, coinSymbol, "SMMA", validateDateTime))
            return null;

        // Initial SMMA is a SMA from previous length records
        decimal smma = coinDataRecords.Skip(length).Average(c => c.Price); 
    
        foreach (var coinData in coinDataRecords.Take(length))
            smma = ((smma * (length - 1)) + coinData.Price) / length;
        
        logger.LogInformation("#INFO {Symbol} {Length} SMMA: {SMMA}", coinSymbol, length, smma);

        return smma;
    }
    
    public async Task<decimal?> CalculateWMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length);

        if (!Validate(coinDataRecords, length, coinSymbol, "WMA", validateDateTime))
            return null;

        decimal currentWeight = length;
        decimal weighting = (decimal)length * (length + 1) / 2;
        decimal wma = 0;
        
        foreach (var coinData in coinDataRecords)
        {
            wma += coinData.Price * (currentWeight / weighting);
            currentWeight--;
        }
        
        logger.LogInformation("#INFO {Symbol} {Length} WMA: {WMA}", coinSymbol, length, wma);

        return wma;
    }

    public async Task<decimal?> CalculateHMA(int length, Guid coinId, string coinSymbol, bool validateDateTime = true)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, length);

        if (!Validate(coinDataRecords, length, coinSymbol, "HMA", validateDateTime))
            return null;

        var halfLength = (int)Math.Round((double)length / 2);
        var halfLengthWMA = CalculateWMA(halfLength, coinDataRecords.Take(halfLength).ToList());
        var fullLengthWMA = CalculateWMA(length, coinDataRecords);

        var rawHma = (2 * halfLengthWMA) - fullLengthWMA;
        var sqrtLength = (int)Math.Sqrt(length);
        
        var hma = CalculateAdaptiveWeightHMA((double)rawHma!.Value, sqrtLength, coinDataRecords.Take(sqrtLength).ToList());
        
        logger.LogInformation("#INFO {Symbol} {Length} HMA: {HMA}", coinSymbol, length, hma);

        return (decimal)hma;
    }
    
    #endregion
    
    #region Private Helpers
    
    private static decimal? CalculateWMA(int length, List<CoinData> coinDataRecords)
    {
        decimal currentWeight = length;
        decimal weighting = (decimal)length * (length + 1) / 2;
        decimal wma = 0;
        
        foreach (var coinData in coinDataRecords)
        {
            wma += coinData.Price * (currentWeight / weighting);
            currentWeight--;
        }
        
        return wma;
    }
    
    //TODO: !(double)
    private double CalculateAdaptiveWeightHMA(double rawHma, int sqrtLength, List<CoinData> coinDataRecords)
    {
        var weightingFactors = new List<double>();
        foreach (var coinData in coinDataRecords)
        {
            var weightingFactor = (2 * Math.Pow(2, sqrtLength)) / (2 * Math.Pow((double) coinData.Price, sqrtLength));
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
    /// <param name="length"></param>
    /// <param name="coinSymbol"></param>
    /// <param name="averageName"></param>
    /// <param name="validateDateTime"></param>
    /// <returns></returns>
    private bool Validate(IReadOnlyCollection<CoinData> coinDataRecords, int length, string coinSymbol, string averageName, bool validateDateTime)
    {
        if (coinDataRecords.Count != length)
        {
            logger.LogError("Database does not contain enough {Symbol} prices. " +
                            "Cannot calculate {AverageName} for expected length: {Length}; actual records {RecordsCount}",
                coinSymbol, averageName, length, coinDataRecords.Count);
            return false;
        }
        
        //TODO: Improve 
        if (!validateDateTime)
            return true;
        
        var lastDate = DateTime.UtcNow.AddMinutes(-length * 5 - 8); //This setting allows for missing one record in this currently working 5 minutes interval
        if (coinDataRecords.Last().DateTime > lastDate)
            return true;
        
        logger.LogError("Database does not contain valid {Symbol} prices. " +
                        "Cannot calculate {AverageName} for length: {Length}",
            coinSymbol, averageName, length);
        return false;
    }
    
    #endregion
}