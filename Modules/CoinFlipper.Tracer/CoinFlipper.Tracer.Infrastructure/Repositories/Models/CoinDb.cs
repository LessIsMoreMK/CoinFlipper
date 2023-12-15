namespace CoinFlipper.Tracer.Infrastructure.Repositories.Models;

public class CoinDb
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Symbol { get; set; } = null!;

    public string CoinGeckoId { get; set; } = null!;
    
    public ICollection<CoinDataDb> CoinData { get; set; } = null!;
}