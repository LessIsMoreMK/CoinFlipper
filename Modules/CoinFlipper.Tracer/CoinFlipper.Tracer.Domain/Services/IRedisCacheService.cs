using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Services;

public interface IRedisCacheService
{
    IReadOnlyCollection<Coin> GetCoins();
    Task AddCoinsAsync();

    Task<List<CoinData>> GetCachedCoinDataListAsync(Guid coinId, int count);
    Task AddCoinDataToDbAndUpdateCacheAsync(List<CoinData> coinDataList);
}