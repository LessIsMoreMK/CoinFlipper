namespace CoinFlipper.Tracer.Domain.Entities;

public class Coin
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Symbol { get; set; } = null!;

    public string CoinGeckoId { get; set; } = null!;

    public Coin(Guid id, string name, string symbol, string coinGeckoId)
    {
        Id = id;
        Name = name;
        Symbol = symbol;
        CoinGeckoId = coinGeckoId;
    }
}