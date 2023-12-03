namespace CoinFlipper.ServiceDefaults.Options;

public class PostgresOptions
{
    public bool Enabled { get; set; }
    public bool MigrateOnStartup { get; set; }
    public string ConnectionString { get; set; } = null!;
}