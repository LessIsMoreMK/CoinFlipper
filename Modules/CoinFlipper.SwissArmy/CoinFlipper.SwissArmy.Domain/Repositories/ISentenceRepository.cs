using CoinFlipper.SwissArmy.Domain.Entities;

namespace CoinFlipper.SwissArmy.Domain.Repositories;

public interface ISentenceRepository
{
    Task<List<SentenceEntity>> GetSentencesAsync(int limit);
}