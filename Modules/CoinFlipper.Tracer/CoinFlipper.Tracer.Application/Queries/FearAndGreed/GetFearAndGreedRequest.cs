using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Queries.FearAndGreed.Handlers;

namespace CoinFlipper.Tracer.Application.Queries.FearAndGreed;

public class GetFearAndGreedRequest : IQuery<GetFearAndGreedResponse>
{
    public int Limit { get; set; }
}