using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Services;

public interface IRedisService
{
    IReadOnlyCollection<Coin> GetCoins();
    Task AddCoinsAsync();
}