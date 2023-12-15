namespace CoinFlipper.Tracer.Infrastructure.Repositories.Models;

public class CoinDataDb
{
    public Guid Id { get; set; }
    
    public Guid CoinId { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Volume24h { get; set; }
    
    public decimal Volume { get; set; }
    
    public decimal MarketCap { get; set; }
    
    public CoinDb Coin { get; set; } = null!;

    public CoinDataDb()
    {
        
    }
}