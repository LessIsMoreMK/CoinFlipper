using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.SwissArmy.Domain.Entities;
using CoinFlipper.SwissArmy.Domain.Repositories;
using FluentValidation;
using Mapster;

namespace CoinFlipper.SwissArmy.Application.Queries.Sentence.Handlers;

public class GetSentencesHandler(
    ISentenceRepository sentenceRepository
    ) : IQueryHandler<GetSentencesRequest, List<SentenceEntity>>
{
    public async Task<List<SentenceEntity>> HandleAsync(
        GetSentencesRequest query, CancellationToken cancellationToken = default)
    {
        var result = await sentenceRepository.GetSentencesAsync(query.Limit);

        return result;
    }
}

public class GetFearAndGreedIndexRequestValidator : AbstractValidator<GetSentencesRequest>
{
    public GetFearAndGreedIndexRequestValidator()
    {
        RuleFor(request => request.Limit)
            .InclusiveBetween(1, 100);
    }
}