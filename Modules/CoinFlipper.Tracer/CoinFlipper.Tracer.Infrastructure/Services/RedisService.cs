using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Domain.Services;

namespace CoinFlipper.Tracer.Infrastructure.Services;

public class RedisService(
    ICoinRepository coinRepository
    ) : IRedisService
{
    //Serves as a current configuration of followed coins
    //Not real Redis just temp solution
    //TODO: RedisService Coins
    #region Coins 
    
    private readonly List<Coin> Coins = new()
    {
        new Coin(new Guid("176de950-d825-4a94-95cc-311c567b92e0"), "Bitcoin", "BTC", "bitcoin"), 
        new Coin(new Guid("c7c47ea0-8f9f-451d-8373-f0e1e6b1d651"), "Ethereum", "ETH", "ethereum"),
        new Coin(new Guid("4eb4bdc3-df4b-4917-8684-99f51094b982"), "Binance Coin", "BNB", "binancecoin"), 
        new Coin(new Guid("2a5b1ff2-4c03-41d0-8fc9-179498b9b264"), "Solana", "SOL", "solana"), 
        new Coin(new Guid("aa0b5a9a-63b4-4e4b-86f1-44a2362ace59"), "Polygon", "MATIC", "matic-network")
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
}