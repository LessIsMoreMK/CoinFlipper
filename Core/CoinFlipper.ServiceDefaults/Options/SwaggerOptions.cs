namespace CoinFlipper.ServiceDefaults.Options;

internal class SwaggerOptions
{
    public bool Enabled { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Version { get; set; } = null!;
}