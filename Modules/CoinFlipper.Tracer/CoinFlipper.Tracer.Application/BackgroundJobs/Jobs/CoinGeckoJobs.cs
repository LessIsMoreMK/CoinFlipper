using CoinFlipper.Shared.DateTimeHelpers;
using CoinFlipper.Tracer.Application.BackgroundJobs.Jobs.Interfaces;
using CoinFlipper.Tracer.Application.Clients;
using CoinFlipper.Tracer.Application.ExternalResponses;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using Hangfire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.BackgroundJobs.Jobs;

public class CoinGeckoJobs(
    ICoinGeckoClient coinGeckoClient,
    ICoinRepository coinRepository,
    ICoinDataRepository coinDataRepository,
    ILogger<CoinGeckoJobs> logger,
    IRecurringJobManager jobManager
    ) : ICoinGeckoJobs
{
    //TODO: CoinGeckoTracerJob Configuration
    private readonly List<Coin> Coins = new()
    {
        new Coin(new Guid("176de950-d825-4a94-95cc-311c567b92e0"), "Bitcoin", "BTC", "bitcoin"), 
        new Coin(new Guid("c7c47ea0-8f9f-451d-8373-f0e1e6b1d651"), "Ethereum", "ETH", "ethereum"),
        // new Coin(new Guid("4eb4bdc3-df4b-4917-8684-99f51094b982"), "Binance Coin", "BNB", "binancecoin"), 
        // new Coin(new Guid("2a5b1ff2-4c03-41d0-8fc9-179498b9b264"), "Solana", "SOL", "solana"), 
        // new Coin(new Guid("aa0b5a9a-63b4-4e4b-86f1-44a2362ace59"), "Polygon", "MATIC", "matic-network"), 
        // new Coin(new Guid("a56c2efb-c982-49cc-aa9f-d93896119e10"), "Injective", "INJ", "injective-protocol") 
    };
    
    #region Methods
    
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

            foreach (var coinPrice in coinsPrices.CoinsPrices)
            {
                var coinId = Coins.First(c => c.CoinGeckoId == coinPrice.Key).Id;
                var newestRecord = (await coinDataRepository.GetCoinDataXNewestRecords(coinId, 1))[0];

                if (DateTimeExtensions.TimestampToDateTime(coinPrice.Value.LastUpdatedAt) == newestRecord.DateTime)
                    break;
                
                await coinDataRepository.AddCoinDataAsync(
                    new CoinData(
                        Guid.NewGuid(),       
                        coinId,              
                        DateTimeExtensions.TimestampToDateTime(coinPrice.Value.LastUpdatedAt),             
                        coinPrice.Value.Usd,       
                        coinPrice.Value.Change24h,
                        Math.Abs(newestRecord.Volume24h - coinPrice.Value.Volume24h),
                        coinPrice.Value.MarketCap                 
                    ));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while processing {CoinGeckoTracerJob}", JobsIdentifier.CoinGeckoTracerJob);
        }
        
        jobManager.Trigger(JobsIdentifier.IndicatorsJob); 
    }

    public async Task InitCoinsAsync()
    {
        foreach (var coin in Coins)
        {
            try
            {
                await coinRepository.AddCoinAsync(coin);
                
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
                    break;
                }
                
                var coinGeckoPriceHistoryResponse = JsonConvert.DeserializeObject<CoinGeckoPriceHistoryResponse>(priceHistoryResponse);

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
                    
                    await coinDataRepository.AddCoinDataAsync(
                        new CoinData(
                            Guid.NewGuid(),       
                            coin.Id,              
                            timestamp,             
                            coinGeckoPriceHistoryResponse.Prices[index][1],       
                            coinGeckoPriceHistoryResponse.TotalVolumes[index][1],
                            periodVolume,
                            coinGeckoPriceHistoryResponse.MarketCaps[index][1]                  
                        ));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while processing {CoinGeckoInitJob} for coin: {Coin}", JobsIdentifier.CoinGeckoInitJob, coin.Symbol);
            }
        }
        
        jobManager.AddOrUpdate<ICoinGeckoJobs>(JobsIdentifier.CoinGeckoTracerJob, job => job.TrackCoinsAsync(), "*/5 * * * *");
    }
    
    #endregion
}