namespace CoinFlipper.ServiceDefaults.Options;

public class RedisOptions
{
    public bool Enabled { get; set; }    
    public string Address { get; set; } = null!;
    public string ServicePrefix { get; set; } = null!;
}