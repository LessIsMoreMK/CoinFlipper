namespace CoinFlipper.ServiceDefaults.Options;

internal class LoggerOptions
{
    public string Level { get; set; } = null!;
    public IEnumerable<string>? ExcludePaths { get; set; }
    
    public ConsoleOptions Console { get; set; } = new();
    public FileOptions File { get; set; } = new();
    public SeqOptions Seq { get; set; } = new();
    
    public Dictionary<string, string> MinimumLevelOverrides { get; set; } = new();
}

internal sealed class ConsoleOptions
{
    public bool Enabled { get; set; }
}

internal sealed class FileOptions
{
    public bool Enabled { get; set; }
    public string Path { get; set; } = null!;
    public string Interval { get; set; } = null!;
}

internal sealed class SeqOptions
{
    public bool Enabled { get; set; }
    public string Url { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}