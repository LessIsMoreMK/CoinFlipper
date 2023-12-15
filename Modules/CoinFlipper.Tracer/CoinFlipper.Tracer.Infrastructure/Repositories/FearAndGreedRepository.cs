using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CoinFlipper.Tracer.Infrastructure.Repositories;

public class FearAndGreedRepository(ApplicationDbContext dbContext) : IFearAndGreedRepository
{
    public async Task<List<FearAndGreed>> GetLastXFearAndGreedAsync(int limit)
    {
        var result = await dbContext.Set<FearAndGreedDb>()
            .AsNoTracking()
            .OrderByDescending(x => x.DateTime)
            .Take(limit)
            .ToListAsync();
        
        return result?.Adapt<List<FearAndGreed>>() ?? new List<FearAndGreed>();
    }

    public async Task AddFearAndGreedAsync(FearAndGreed fearAndGreed)
    {
        var fearAndGreedDb = fearAndGreed.Adapt<FearAndGreedDb>();
        dbContext.FearAndGreed.Add(fearAndGreedDb);
        await dbContext.SaveChangesAsync();
    }
}