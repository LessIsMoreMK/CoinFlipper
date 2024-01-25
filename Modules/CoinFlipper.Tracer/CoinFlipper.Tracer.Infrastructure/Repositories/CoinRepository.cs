using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Repositories;
using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CoinFlipper.Tracer.Infrastructure.Repositories;

public class CoinRepository(ApplicationDbContext dbContext) : ICoinRepository
{
    public async Task<Coin?> GetCoinBySymbol(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol)) 
            throw new ArgumentNullException(nameof(symbol));

        var coinDb = await dbContext.Coin
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Symbol == symbol);
        
        return coinDb?.Adapt<Coin>();
    }
    
    public async Task AddCoinAsync(Coin coin)
    {
        if (coin == null) 
            throw new ArgumentNullException(nameof(coin));

        var existingCoin = await GetCoinBySymbol(coin.Symbol);
        if (existingCoin != null)
            return;
        
        await dbContext.Coin.AddAsync(coin.Adapt<CoinDb>());
        await dbContext.SaveChangesAsync();
    }
}