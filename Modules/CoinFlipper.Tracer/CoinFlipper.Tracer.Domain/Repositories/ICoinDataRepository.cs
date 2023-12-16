using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Repositories;

public interface ICoinDataRepository
{
    Task<List<CoinData>> GetCoinDataXNewestRecords(Guid coinId, int x);
    
    Task AddCoinDataAsync(CoinData coinData);
    
    Task AddCoinDataListAsync(List<CoinData> coinDataList);
}