using CoinFlipper.Tracer.Application.Dtos;

namespace CoinFlipper.Tracer.Application.Queries.FearAndGreed.Handlers;

public class GetFearAndGreedResponse
{
    public List<FearAndGreedDto> FearAndGreedDtos { get; set; } = null!;
    
    public string Source { get; } = "https://alternative.me";
}