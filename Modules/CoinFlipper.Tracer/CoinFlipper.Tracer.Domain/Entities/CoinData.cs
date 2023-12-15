namespace CoinFlipper.Tracer.Domain.Entities;

public class CoinData
{
    public Guid Id { get; set; }
    
    public Guid CoinId { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Volume24h { get; set; }
    
    public decimal Volume { get; set; }
    
    public decimal MarketCap { get; set; }

    public CoinData()
    {
        
    }

    public CoinData(Guid id, Guid coinId, DateTime dateTime, decimal price, decimal volume24H, decimal volume, decimal marketCap)
    {
        Id = id;
        CoinId = coinId;
        DateTime = dateTime;
        Price = price;
        Volume24h = volume24H;
        Volume = volume;
        MarketCap = marketCap;
    }
}