using Newtonsoft.Json;

namespace CoinFlipper.Tracer.Application.Responses;

public record FearAndGreedResponse(
    string Name,
    List<Data> Data,
    Metadata Metadata
    );

public record Data(
    int Value,
    [JsonProperty(PropertyName = "value_classification")] string Classification,
    long Timestamp,
    [JsonProperty(PropertyName = "time_until_update")] long TimeUntilUpdate
    );

public record Metadata(
    object Error
    );

