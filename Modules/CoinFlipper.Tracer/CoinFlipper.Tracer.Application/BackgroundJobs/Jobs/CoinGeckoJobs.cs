using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.ExternalResponses;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Domain.Services;
using Hangfire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class CoinGeckoJobs(
    ICoinGeckoClient coinGeckoClient,
    ICoinDataRepository coinDataRepository,
    ILogger<CoinGeckoJobs> logger,
    IRecurringJobManager jobManager,
    IRedisService redisService
    ) : ICoinGeckoJobs
{
    private IReadOnlyCollection<Coin> Coins = null!;
    
    #region Methods
    
    public async Task InitCoinsAsync()
    {
        await redisService.AddCoinsAsync();
        Coins = redisService.GetCoins();
        
        foreach (var coin in Coins)
            await InitCoinAsync(coin);
        
        jobManager.AddOrUpdate<ICoinGeckoJobs>(JobsIdentifier.CoinGeckoTracerJob, job => job.TrackCoinsAsync(), "*/5 * * * *");
    }

    private async Task InitCoinAsync(Coin coin)
    {
        try
        {
            var newestCoinDataRecord = await coinDataRepository.GetCoinDataXNewestRecords(coin.Id, 1);

            var utcNow = DateTime.UtcNow;
            var fromDate = newestCoinDataRecord.Count == 0 || newestCoinDataRecord[0].DateTime < utcNow.Date.AddMinutes(-5)
                ? utcNow.AddDays(-1)
                : newestCoinDataRecord[0].DateTime.AddSeconds(1);
            
            var priceHistoryResponse = await coinGeckoClient.GetCoinPriceHistory(coin.CoinGeckoId, 
                DateTimeExtensions.DateTimeToTimestamp(fromDate), DateTimeExtensions.DateTimeToTimestamp(utcNow));
            if (priceHistoryResponse is null)
            {
                logger.LogError("Unable to obtain {Coin} price history", coin.Symbol);
                return;
            }
            
            var coinGeckoPriceHistoryResponse = JsonConvert.DeserializeObject<CoinGeckoPriceHistoryResponse>(priceHistoryResponse);

            var coinDataList = new List<CoinData>();
            foreach (var index in Enumerable.Range(0, coinGeckoPriceHistoryResponse.Prices.Count))
            {
                decimal volumeDifference;
                if (index == 0)
                    volumeDifference = newestCoinDataRecord.Count == 0 
                        ? 0 //Cannot calculate period volume
                        : newestCoinDataRecord[0].Volume24h - coinGeckoPriceHistoryResponse.TotalVolumes[index][1];
                else
                    volumeDifference = coinGeckoPriceHistoryResponse.TotalVolumes[index - 1][1] - coinGeckoPriceHistoryResponse.TotalVolumes[index][1];
                var periodVolume = Math.Abs(volumeDifference);

                var timestamp = DateTimeExtensions.TimestampToDateTime((long) coinGeckoPriceHistoryResponse.Prices[index][0], true);
                
                coinDataList.Add(new CoinData(
                    Guid.NewGuid(),       
                    coin.Id,              
                    timestamp,             
                    coinGeckoPriceHistoryResponse.Prices[index][1],       
                    coinGeckoPriceHistoryResponse.TotalVolumes[index][1],
                    periodVolume,
                    coinGeckoPriceHistoryResponse.MarketCaps[index][1]                  
                ));
            }
            
            if (coinDataList.Count != 0)
                await coinDataRepository.AddCoinDataListAsync(coinDataList);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while processing {CoinGeckoInitJob} for coin: {Coin}", JobsIdentifier.CoinGeckoInitJob, coin.Symbol);
        }
    }
    
    public async Task TrackCoinsAsync()
    {
        try
        {
            var coinIds = string.Join(",", Coins.Select(c => c.CoinGeckoId));
            var coinsPricesResponse = await coinGeckoClient.GetCoinsPrice(coinIds);
            if (coinsPricesResponse is null)
            {
                logger.LogError("Unable to obtain current prices");
                return;
            }
            
            var coinsPrices = JsonHelpers.JsonHelpers.DeserializeCoinGeckoCoinPrices(coinsPricesResponse);

            var coinDataList = new List<CoinData>();
            foreach (var coinPrice in coinsPrices.CoinsPrices)
            {
                var coinId = Coins.First(c => c.CoinGeckoId == coinPrice.Key).Id;
                var newestRecord = (await coinDataRepository.GetCoinDataXNewestRecords(coinId, 1))[0];

                if (DateTimeExtensions.TimestampToDateTime(coinPrice.Value.LastUpdatedAt) == newestRecord.DateTime)
                    break;
                
                coinDataList.Add(new CoinData(
                    Guid.NewGuid(),       
                    coinId,              
                    DateTimeExtensions.TimestampToDateTime(coinPrice.Value.LastUpdatedAt),             
                    coinPrice.Value.Usd,       
                    coinPrice.Value.Volume24h,
                    Math.Abs(newestRecord.Volume24h - coinPrice.Value.Volume24h),
                    coinPrice.Value.MarketCap                 
                ));
            }
            if (coinDataList.Count != 0)
                await coinDataRepository.AddCoinDataListAsync(coinDataList);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while processing {CoinGeckoTracerJob}", JobsIdentifier.CoinGeckoTracerJob);
        }
        
        jobManager.Trigger(JobsIdentifier.IndicatorsJob); 
    }
    
    #endregion
}