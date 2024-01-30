using System.Globalization;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Domain.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Infrastructure.Services;

//TODO: Unit tests
public class RedisCacheService(
    ICoinRepository coinRepository,
    ICoinDataRepository coinDataRepository,
    IDistributedCache distributedCache,
    ILogger<RedisCacheService> logger
    ) : IRedisCacheService
{
    //Serves as a current configuration of followed coins
    //Not real Redis just temp solution
    //TODO: RedisService Coins
    #region Coins 
    
    private readonly List<Coin> Coins = new()
    {
        new Coin(new Guid("176de950-d825-4a94-95cc-311c567b92e0"), "Bitcoin", "BTC", "bitcoin"), 
        new Coin(new Guid("c7c47ea0-8f9f-451d-8373-f0e1e6b1d651"), "Ethereum", "ETH", "ethereum"),
        // new Coin(new Guid("4eb4bdc3-df4b-4917-8684-99f51094b982"), "Binance Coin", "BNB", "binancecoin"), 
        // new Coin(new Guid("2a5b1ff2-4c03-41d0-8fc9-179498b9b264"), "Solana", "SOL", "solana"), 
        // new Coin(new Guid("aa0b5a9a-63b4-4e4b-86f1-44a2362ace59"), "Polygon", "MATIC", "matic-network")
    };
    
    public async Task AddCoinsAsync()
    {
        foreach (var coin in Coins)
            await coinRepository.AddCoinAsync(coin);
    }
    
    public IReadOnlyCollection<Coin> GetCoins()
    {
        return Coins;
    }
    
    #endregion
    
    #region CoinsData

    private const string CoinDataCacheKeySuffix = "_coindata";

    public async Task<List<CoinData>> GetCoinDataListAsync(Guid coinId, int count)
    {
        var cacheKey = $"{coinId}{CoinDataCacheKeySuffix}";
        var cachedCoinData = await distributedCache.GetStringAsync(cacheKey);

        List<CoinData> coinDataList;
        if (!string.IsNullOrWhiteSpace(cachedCoinData))
        {
            coinDataList = JsonConvert.DeserializeObject<List<CoinData>>(cachedCoinData);
            
            if (coinDataList != null && coinDataList.Count >= count)
                return coinDataList.Take(count).ToList();
        }

        coinDataList = await coinDataRepository.GetCoinDataXNewestRecords(coinId, count);
        
        await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(coinDataList), 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)}
            );

        return coinDataList;
    }

    public async Task AddCoinDataToDbAndUpdateCacheAsync(List<CoinData> coinDataList)
    {
        if (coinDataList == null || coinDataList.Count == 0)
            throw new ArgumentNullException(nameof(coinDataList));

        coinDataList = coinDataList.OrderByDescending(coinData => coinData.DateTime).ToList();
        
        await coinDataRepository.AddCoinDataListAsync(coinDataList);

        foreach (var group in coinDataList.GroupBy(cd => cd.CoinId))
            await UpdateCacheForCoinAsync(group.Key, group.ToList());
    }

    private async Task UpdateCacheForCoinAsync(Guid coinId, List<CoinData> newCoinDataList)
    {
        var cacheKey = $"{coinId}{CoinDataCacheKeySuffix}";
        var cachedCoinData = await distributedCache.GetStringAsync(cacheKey);

        var currentCachedCoinData = string.IsNullOrWhiteSpace(cachedCoinData) 
            ? new List<CoinData>() 
            : JsonConvert.DeserializeObject<List<CoinData>>(cachedCoinData);

        currentCachedCoinData.InsertRange(0, newCoinDataList);
        
        if (currentCachedCoinData.Count > 300) 
            currentCachedCoinData = currentCachedCoinData.Take(300).ToList();

        await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(currentCachedCoinData), 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
    }

    #endregion
    
    #region Methods

    private const int MaxElements = 288;
    private const char Separator = ';';
    
    public async Task StackValueAsync(string cacheKey, string value, TimeSpan? expiry = null)
    {
        try
        {
            var existingValue = await distributedCache.GetStringAsync(cacheKey);

            existingValue = existingValue == null ? value : value + Separator + existingValue;

            var elementsArray = existingValue.Split(Separator);
            if (elementsArray.Length > MaxElements)
                existingValue = string.Join(Separator, elementsArray.Take(MaxElements));
            
            await distributedCache.SetStringAsync(cacheKey, existingValue,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromDays(7) 
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while stacking value to redis cache for key: {CacheKey}", cacheKey);
            throw;
        }
    }
    
    public async Task ReplaceValueAsync(string cacheKey, string value, TimeSpan? expiry = null)
    {
        await distributedCache.SetStringAsync(cacheKey, value,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromDays(7) 
            });
    }

    public async Task<string?> GetStringAsync(string cacheKey)
        => await distributedCache.GetStringAsync(cacheKey);

    public async Task<string[]> GetStringArrayAsync(string cacheKey)
    {
        var existingValue = await distributedCache.GetStringAsync(cacheKey);
        var stringArray = existingValue?.Split(Separator);
        return stringArray ?? Array.Empty<string>();
    }
    
    public async Task<decimal[]> GetDecimalArrayAsync(string cacheKey)
    {
        var stringArray = await GetStringArrayAsync(cacheKey);
        var decimalArray = stringArray.Select(element => decimal.Parse(element, CultureInfo.InvariantCulture)).ToArray();
        return decimalArray;
    }
    
    #endregion
}