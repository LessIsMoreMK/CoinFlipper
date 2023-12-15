using CoinFlipper.Tracer.Application.ExternalResponses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoinFlipper.Tracer.Application.JsonHelpers;

public static class JsonHelpers
{
    public static CoinPricesResponse DeserializeCoinGeckoCoinPrices(string response)
    {
        var data = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(response);
        var coinsPrices = new Dictionary<string, CoinPricesData>();

        foreach (var kvp in data)
            coinsPrices.Add(kvp.Key, kvp.Value.ToObject<CoinPricesData>());

        return new CoinPricesResponse(coinsPrices);
    }
}