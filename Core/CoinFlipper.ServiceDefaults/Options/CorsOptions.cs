namespace CoinFlipper.ServiceDefaults.Options;

internal class CorsOptions
{
    public bool Enabled { get; set; }
    
    public CorsPolicyOptions[] Policies { get; set; } = null!;
}

internal class CorsPolicyOptions
{
    public string Name { get; set; } = null!;

    public bool? AllowAnyOrigin { get; set; }

    public string[] AllowedOrigins { get; set; } = null!;

    public bool? AllowAnyMethod { get; set; }

    public string[] AllowedMethods { get; set; } = null!;

    public bool? AllowAnyHeader { get; set; }

    public string[] AllowedHeaders { get; set; } = null!;

    public bool? AllowCredentials { get; set; }

    public string[] ExposedHeaders { get; set; } = null!;
}