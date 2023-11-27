using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CoinFlipper.Tracer.Infrastructure.Repositories;

public class FearAndGreedRepository : IFearAndGreedRepository
{
    #region Setup

    private readonly ApplicationDbContext _dbContext;
    
    public FearAndGreedRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    #endregion

    #region Methods
    
    public async Task<List<FearAndGreed>> GetLastXFearAndGreedAsync(int limit)
    {
        return new List<FearAndGreed>() { new FearAndGreed()
        {
            Value = 1
        }};
        
        var result = await _dbContext.Set<FearAndGreedDb>()
            .TakeLast(limit)
            .AsNoTracking()
            .ToListAsync();
        
        return result?.Adapt<List<FearAndGreed>>() ?? new List<FearAndGreed>();
    }

    public async Task AddFearAndGreedAsync(FearAndGreed fearAndGreed)
    {
        var fearAndGreedDb = fearAndGreed.Adapt<FearAndGreedDb>();
        _dbContext.FearAndGreed.Add(fearAndGreedDb);
        await _dbContext.SaveChangesAsync();
    }
    
    #endregion
}