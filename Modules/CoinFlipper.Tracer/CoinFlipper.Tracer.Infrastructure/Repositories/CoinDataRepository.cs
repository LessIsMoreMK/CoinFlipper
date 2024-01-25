using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CoinFlipper.Tracer.Infrastructure.Repositories;

public class CoinDataRepository(ApplicationDbContext dbContext) : ICoinDataRepository
{
    public async Task<List<CoinData>> GetCoinDataXNewestRecords(Guid coinId, int x)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(x, 1);
        
        if (coinId == Guid.Empty) 
            throw new ArgumentException(null, nameof(coinId));
        
        var lastXRecords = await dbContext.CoinData
            .OrderByDescending(r => r.DateTime) 
            .Where(c => c.CoinId == coinId)
            .Take(x) 
            .AsNoTracking()
            .ToListAsync();
        
        return lastXRecords.Adapt<List<CoinData>>();
    }
    
    public async Task AddCoinDataAsync(CoinData coinData)
    {
        ArgumentNullException.ThrowIfNull(coinData);

        await dbContext.CoinData.AddAsync(coinData.Adapt<CoinDataDb>());
        await dbContext.SaveChangesAsync();
    }
    
    public async Task AddCoinDataListAsync(List<CoinData> coinDataList)
    {
        if (coinDataList == null || coinDataList.Count == 0) 
            throw new ArgumentNullException(nameof(coinDataList));
    
        var coinDataDbList = coinDataList.Select(coinData => coinData.Adapt<CoinDataDb>()).ToList();
        await dbContext.CoinData.AddRangeAsync(coinDataDbList);
        await dbContext.SaveChangesAsync();
    }
}