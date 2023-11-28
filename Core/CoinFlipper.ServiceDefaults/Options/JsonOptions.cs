using System.Text.Json;

namespace CoinFlipper.ServiceDefaults.Options;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}