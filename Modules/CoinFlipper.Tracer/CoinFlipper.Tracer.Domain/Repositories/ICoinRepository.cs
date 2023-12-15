using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Repositories;

public interface ICoinRepository
{
    public Task<Coin?> GetCoinBySymbol(string symbol);
    
    public Task AddCoinAsync(Coin coin);
}