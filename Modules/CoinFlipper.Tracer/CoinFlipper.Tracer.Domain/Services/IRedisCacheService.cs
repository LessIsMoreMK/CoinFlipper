using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Services;

public interface IRedisCacheService
{
    IReadOnlyCollection<Coin> GetCoins();
    Task AddCoinsAsync();

    
    Task<List<CoinData>> GetCoinDataListAsync(Guid coinId, int count);
    Task AddCoinDataToDbAndUpdateCacheAsync(List<CoinData> coinDataList);
    

    Task StackValueAsync(string cacheKey, string value, TimeSpan? expiry = null);
    
    Task<string?> GetStringAsync(string cacheKey);
    Task<string[]> GetStringArrayAsync(string cacheKey);
    Task<decimal[]> GetDecimalArrayAsync(string cacheKey);
}