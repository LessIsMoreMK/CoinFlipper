namespace CoinFlipper.Tracer.Infrastructure.Repositories.Models;

public class FearAndGreedDb
{
    public DateTime DateTime { get; set; }
    
    public int Value { get; set; }
    
    public string Classification { get; set; } = null!;
}