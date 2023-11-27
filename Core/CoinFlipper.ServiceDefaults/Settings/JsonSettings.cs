using System.Text.Json;

namespace CoinFlipper.ServiceDefaults.Settings;

public static class JsonSettings
{
    public static readonly JsonSerializerOptions DefaultSettings = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}