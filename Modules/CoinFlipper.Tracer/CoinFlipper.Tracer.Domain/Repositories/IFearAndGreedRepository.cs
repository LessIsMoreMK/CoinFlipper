using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Domain.Repositories;

public interface IFearAndGreedRepository
{
    Task<List<FearAndGreed>> GetLastXFearAndGreedAsync(int limit);

    Task AddFearAndGreedAsync(FearAndGreed fearAndGreed);
}