using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.Services.Indicators;

public class RsiIndicatorService(
    ILogger<RsiIndicatorService> logger,
    IRedisCacheService redisCacheService
    ) : IRsiIndicatorService
{
    private const int Length = 14;
    private const int RecordsForCalculation = 288;
    
    public async Task<decimal?> CalculateRSI(Guid coinId, string coinSymbol)
    {
        var coinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, RecordsForCalculation);
        coinDataRecords.Reverse();
        
        if (coinDataRecords.Count < RecordsForCalculation)
        {
            logger.LogError("Database does not contain enough {Symbol} prices. " +
                            "Cannot calculate RSI for expected RecordsForCalculation: {RecordsForCalculation}; actual records {RecordsCount}",
                coinSymbol, RecordsForCalculation, coinDataRecords.Count);
            return null;
        }

        var positiveChanges = new decimal[RecordsForCalculation];
        var negativeChanges = new decimal[RecordsForCalculation];
        var averageGain = new decimal[RecordsForCalculation];
        var averageLoss = new decimal[RecordsForCalculation];
        var rsi = new decimal[RecordsForCalculation];
        
        for (var i = 0; i < RecordsForCalculation; i++)
        {
            var currentDifference = 0.0m;
            if (i > 0)
            {
                var previousClose = coinDataRecords[i-1].Price;
                var currentClose = coinDataRecords[i].Price;
                currentDifference = currentClose - previousClose;
            }
            positiveChanges[i] = currentDifference > 0 ? currentDifference : 0;
            negativeChanges[i] = currentDifference < 0 ? currentDifference * -1 : 0;

            if (i == Math.Max(1, Length))
            {
                var gainSum = 0.0m;
                var lossSum = 0.0m;
                for(var x = Math.Max(1, Length); x > 0; x--)
                {
                    gainSum += positiveChanges[x];
                    lossSum += negativeChanges[x];
                }

                averageGain[i] = gainSum / Math.Max(1, Length);
                averageLoss[i] = lossSum / Math.Max(1, Length);
            }
            else if (i > Math.Max(1, Length))
            {
                averageGain[i] = ( averageGain[i-1]*(Length-1) + positiveChanges[i]) / Math.Max(1, Length);
                averageLoss[i] = ( averageLoss[i-1]*(Length-1) + negativeChanges[i]) / Math.Max(1, Length);

                if (averageLoss[i] == 0)
                    rsi[i] = 100;
                else if (averageGain[i] == 0)
                    rsi[i] = 0;
                else 
                    rsi[i] = Math.Round(100 - (100 / (1 + averageGain[i] / averageLoss[i])), 2);
            }
        }

        logger.LogInformation("#INFO {Symbol} RSI: {RSI}", coinSymbol, rsi[^1]);

        return rsi[^1];
    }
}