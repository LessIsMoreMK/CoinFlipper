using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class AnalyzersJobs(
    ILogger<IndicatorsJobs> logger,
    IRedisCacheService redisCacheService
    ) : IAnalyzersJobs
{
    #region Methods
    
    private IReadOnlyCollection<Coin> Coins = null!;
    
    public async Task AnalyzeIndicatorsAsync()
    {
        Coins = redisCacheService.GetCoins();
        
        await AnalyzeRsi();
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Logs #SIGNAL when RSI is below 30 or above 70. v.0.1
    /// </summary>
    private async Task AnalyzeRsi()
    {
        foreach (var coin in Coins)
        {
            try
            {
                var cacheKey = $"{coin.Id}_5m_14_rsi";
                var rsi = await redisCacheService.GetDecimalArrayAsync(cacheKey);
                
                if (rsi.Length > 0 && rsi[0] is > 70 or < 30)
                    logger.LogInformation("#SIGNAL {Symbol} 14 RSI: {RSI}", coin.Symbol, rsi[0]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while analyzing RSI for {Symbol}", coin.Symbol);
            }
        }
    }
    
    #endregion
}