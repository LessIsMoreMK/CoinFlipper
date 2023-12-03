using CoinFlipper.SwissArmy.Domain.Entities;
using CoinFlipper.SwissArmy.Domain.Repositories;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Models;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CoinFlipper.SwissArmy.Infrastructure.Repositories;

public class SentenceRepository(ApplicationDbContext dbContext) : ISentenceRepository
{
    public async Task<List<SentenceEntity>> GetSentencesAsync(int limit)
    {
        var sentences = await dbContext.Set<SentenceDb>()
            .AsNoTracking()
            .ToListAsync();

        if (sentences.Count == 0)
            return new List<SentenceEntity>();

        //Randomize 
        var random = new Random();
        var shuffledSentences = sentences.OrderBy(_ => random.Next()).ToList();
        var result = shuffledSentences.Take(Math.Min(limit, shuffledSentences.Count));
        
        return result?.Adapt<List<SentenceEntity>>() ?? new List<SentenceEntity>();
    }
}