using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Repositories;

public interface ICoinDataRepository
{
    public Task<List<CoinData>> GetCoinDataXNewestRecords(Guid coinId, int x);
    
    public Task AddCoinDataAsync(CoinData coinData);
}