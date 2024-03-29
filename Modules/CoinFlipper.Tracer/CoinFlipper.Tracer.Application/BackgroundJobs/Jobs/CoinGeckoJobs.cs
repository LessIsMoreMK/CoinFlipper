using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Shared.Exceptions;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.ExternalResponses;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class CoinGeckoJobs(
    ICoinGeckoClient coinGeckoClient,
    ILogger<CoinGeckoJobs> logger,
    IRedisCacheService redisCacheService
    ) : ICoinGeckoJobs
{
    private IReadOnlyCollection<Coin> Coins = null!;
    
    #region Methods
    
    public async Task InitCoinsAsync()
    {
        await redisCacheService.AddCoinsAsync();
        Coins = redisCacheService.GetCoins();
        
        foreach (var coin in Coins)
            await InitCoinAsync(coin);
    }

    private async Task InitCoinAsync(Coin coin)
    {
        try
        {
            var newestCoinDataRecord = await redisCacheService.GetCoinDataListAsync(coin.Id, 1);

            var utcNow = DateTime.UtcNow;
            var fromDate = newestCoinDataRecord.Count == 0 || newestCoinDataRecord[0].DateTime < utcNow.Date.AddDays(-1)
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
                await redisCacheService.AddCoinDataToDbAndUpdateCacheAsync(coinDataList);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while processing {CoinGeckoInitJob} for coin: {Coin}", JobsIdentifier.CoinGeckoInitJob, coin.Symbol);
        }
    }
    
    public async Task TrackCoinsAsync()
    {
        Coins = redisCacheService.GetCoins();

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
                var newestRecord = (await redisCacheService.GetCoinDataListAsync(coinId, 1))[0];

                if (newestRecord.Price == coinPrice.Value.Usd)
                    throw new RetryException("CoinGecko prices not yet updated.");

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
                await redisCacheService.AddCoinDataToDbAndUpdateCacheAsync(coinDataList);
        }
        catch (Exception ex)
        {
            if (ex is not RetryException)
                logger.LogError(ex, "Error occured while processing {CoinGeckoTracerJob}", JobsIdentifier.CoinGeckoTracerJob);
            
            throw;
        }
    }

    #endregion
}