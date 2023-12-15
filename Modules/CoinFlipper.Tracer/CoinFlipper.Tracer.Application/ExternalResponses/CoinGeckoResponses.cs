using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.ExternalResponses;

public record CoinGeckoCoinListResponse(List<CoinGeckoCoin> Coins);
public record CoinGeckoCoin(
    string Id,
    string Symbol,
    string Name
    );
    
    
public record CoinGeckoPriceHistoryResponse(
    [JsonProperty(PropertyName = "prices")] List<List<decimal>> Prices,
    [JsonProperty(PropertyName = "market_caps")] List<List<decimal>> MarketCaps,
    [JsonProperty(PropertyName = "total_volumes")] List<List<decimal>> TotalVolumes
    );


public record CoinPricesResponse(Dictionary<string, CoinPricesData> CoinsPrices);
public record CoinPricesData(
    [JsonProperty("usd")] decimal Usd,
    [JsonProperty("usd_market_cap")] decimal MarketCap,
    [JsonProperty("usd_24h_vol")] decimal Volume24h,
    [JsonProperty("usd_24h_change")] decimal Change24h,
    [JsonProperty("last_updated_at")] long LastUpdatedAt
    );
