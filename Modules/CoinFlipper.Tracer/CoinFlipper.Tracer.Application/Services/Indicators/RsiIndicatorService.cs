using System.Globalization;
using CoinFlipper.Shared.Exceptions;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Domain.Services.Indicators;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.Services.Indicators;

public class RsiIndicatorService(
    ILogger<RsiIndicatorService> logger,
    IRedisCacheService redisCacheService
    ) : IRsiIndicatorService
{
    #region Fields
    
    private const int Length = 14;
    private const int RecordsForCalculation = 288;
    
    #endregion

    #region Methods
    
    public async Task<decimal> CalculateRSI(Guid coinId, string coinSymbol)
    {
        var r = await Prepare(coinId, coinSymbol);
        
        for (var i = r.ContinueFromIndex; i < RecordsForCalculation; i++)
        {
            var currentDifference = 0.0m;
            if (i > 0)
            {
                var previousClose = r.CoinDataRecords[i-1].Price;
                var currentClose = r.CoinDataRecords[i].Price;
                currentDifference = currentClose - previousClose;
            }
            r.PositiveChanges[i] = currentDifference > 0 ? currentDifference : 0;
            r.NegativeChanges[i] = currentDifference < 0 ? currentDifference * -1 : 0;

            if (i == Math.Max(1, Length))
            {
                var gainSum = 0.0m;
                var lossSum = 0.0m;
                for(var x = Math.Max(1, Length); x > 0; x--)
                {
                    gainSum += r.PositiveChanges[x];
                    lossSum += r.NegativeChanges[x];
                }

                r.AverageGain[i] = gainSum / Math.Max(1, Length);
                r.AverageLoss[i] = lossSum / Math.Max(1, Length);
            }
            else if (i > Math.Max(1, Length))
            {
                r.AverageGain[i] = (r.AverageGain[i-1]*(Length-1) + r.PositiveChanges[i]) / Math.Max(1, Length);
                r.AverageLoss[i] = (r.AverageLoss[i-1]*(Length-1) + r.NegativeChanges[i]) / Math.Max(1, Length);

                if (r.AverageLoss[i] == 0)
                    r.Rsi[i] = 100;
                else if (r.AverageGain[i] == 0)
                    r.Rsi[i] = 0;
                else 
                    r.Rsi[i] = Math.Round(100 - (100 / (1 + r.AverageGain[i] / r.AverageLoss[i])), 2);
            }
            
            await redisCacheService.StackValueAsync(r.PositiveChangesKey, r.PositiveChanges[i].ToString(CultureInfo.InvariantCulture));
            await redisCacheService.StackValueAsync(r.NegativeChangesKey, r.NegativeChanges[i].ToString(CultureInfo.InvariantCulture));
            await redisCacheService.StackValueAsync(r.AverageGainKey, r.AverageGain[i].ToString(CultureInfo.InvariantCulture));
            await redisCacheService.StackValueAsync(r.AverageLossKey, r.AverageLoss[i].ToString(CultureInfo.InvariantCulture));
            await redisCacheService.StackValueAsync(r.RsiKey, r.Rsi[i].ToString(CultureInfo.InvariantCulture));
        }

        logger.LogInformation("#INFO {Symbol} RSI: {RSI}", coinSymbol, r.Rsi[^1]);
        
        //All processed so update the LastUpdateKey
        await redisCacheService.ReplaceValueAsync(r.LastUpdatedKey, r.CoinDataRecords[^1].DateTime.Ticks.ToString(CultureInfo.InvariantCulture));
        
        return r.Rsi[^1];
    }
    
    #endregion
    
    #region Private Helpers
    
    private async Task<RsiComponents> Prepare(Guid coinId, string coinSymbol)
    {
        var r = new RsiComponents(coinId, RecordsForCalculation)
        {
            CoinDataRecords = await redisCacheService.GetCoinDataListAsync(coinId, RecordsForCalculation)
        };

        if (r.CoinDataRecords.Count < RecordsForCalculation)
            throw new NotEnoughDataException($"Not enough records for calculating {coinSymbol} RSI. Actual: {r.CoinDataRecords.Count} Expected: {RecordsForCalculation}");
        
        r.CoinDataRecords.Reverse();
        r.ContinueFromIndex = await CalculateContinueFromIndex(r);
        await UpdateRsiComponentsArrays(r, r.ContinueFromIndex);

        return r;
    }
  
    private async Task<int> CalculateContinueFromIndex(RsiComponents r)
    {
        var lastAnalyzedRecordDateTimeTicks = await redisCacheService.GetStringAsync(r.LastUpdatedKey);
        
        var index = r.CoinDataRecords.FindIndex(c => c.DateTime.Ticks.ToString() == lastAnalyzedRecordDateTimeTicks);
        
        return index == -1 ? 0 : index+1;
    }
    
    private async Task UpdateRsiComponentsArrays(RsiComponents r, int continueFromIndex)
    {
        var positiveChanges = await redisCacheService.GetDecimalArrayAsync(r.PositiveChangesKey);
        var negativeChanges = await redisCacheService.GetDecimalArrayAsync(r.NegativeChangesKey);
        var averageGain = await redisCacheService.GetDecimalArrayAsync(r.AverageGainKey);
        var averageLoss = await redisCacheService.GetDecimalArrayAsync(r.AverageLossKey);
        var rsi = await redisCacheService.GetDecimalArrayAsync(r.RsiKey);

        var ii = continueFromIndex - 1;

        if (positiveChanges.Length < ii) //When not enough cached continue from what stored
            r.ContinueFromIndex = positiveChanges.Length;
        
        for (var i = 0; i < continueFromIndex; i++, ii--)
        {
            r.PositiveChanges[i] = positiveChanges[ii];
            r.NegativeChanges[i] = negativeChanges[ii];
            r.AverageGain[i] = averageGain[ii];
            r.AverageLoss[i] = averageLoss[ii];
            r.Rsi[i] = rsi[ii];
        }
    }
    
    #endregion
}

public class RsiComponents(Guid coinId, int recordsForCalculation)
{
    public int ContinueFromIndex { get; set; }
    public List<CoinData> CoinDataRecords { get; set; }
    
    public decimal[] PositiveChanges { get; set; } = new decimal[recordsForCalculation];
    public decimal[] NegativeChanges { get; set; } = new decimal[recordsForCalculation];
    public decimal[] AverageGain { get; set; } = new decimal[recordsForCalculation];
    public decimal[] AverageLoss { get; set; } = new decimal[recordsForCalculation];
    public decimal[] Rsi { get; set; } = new decimal[recordsForCalculation];
    
    public string LastUpdatedKey { get; } = $"{coinId}_5m_lastUpdated_rsi";
    public string PositiveChangesKey { get; } = $"{coinId}_5m_14_positiveChanges_rsi";
    public string NegativeChangesKey { get; } = $"{coinId}_5m_14_negativeChanges_rsi";
    public string AverageGainKey { get; } = $"{coinId}_5m_14_averageGain_rsi";
    public string AverageLossKey { get; } = $"{coinId}_5m_14_averageLoss_rsi";
    public string RsiKey { get; } = $"{coinId}_5m_14_rsi";
}