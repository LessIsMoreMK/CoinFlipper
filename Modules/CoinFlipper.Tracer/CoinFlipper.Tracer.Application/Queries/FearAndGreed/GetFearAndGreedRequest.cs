using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Dtos;

namespace CoinFlipper.Tracer.Application.Queries.FearAndGreed;

public class GetFearAndGreedRequest : IQuery<List<FearAndGreedDto>>
{
    public int Limit { get; set; }
}