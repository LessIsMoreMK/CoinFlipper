using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Dtos;
using CoinFlipper.Tracer.Domain.Repositories;
using Mapster;

namespace CoinFlipper.Tracer.Application.Queries.FearAndGreed.Handlers;

public class GetFearAndGreedIndexHandler(
    IFearAndGreedRepository fearAndGreedRepository
    ) : IQueryHandler<GetFearAndGreedRequest, List<FearAndGreedDto>>
{
    public async Task<List<FearAndGreedDto>> HandleAsync(
        GetFearAndGreedRequest query, CancellationToken cancellationToken = default)
    {
        var result = await fearAndGreedRepository.GetLastXFearAndGreedAsync(query.Limit);

        return result?.Adapt<List<FearAndGreedDto>>() ?? new List<FearAndGreedDto>();
    }
}