using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.SwissArmy.Domain.Entities;

namespace CoinFlipper.SwissArmy.Application.Queries.Sentence;

public class GetSentencesRequest : IQuery<List<SentenceEntity>>
{
    public int Limit { get; set; }
}