namespace CoinFlipper.Tracer.Application.Clients;

/// <summary>
/// Http client for alternative.me
/// </summary>
public interface IFearAndGreedIndexClient
{
    /// <summary>
    /// Gets the fear and greed index from alternative.me
    /// </summary>
    /// <param name="limit">Limit in days</param>
    /// <param name="csvFormat">Data format in csv; json otherwise</param>
    /// <returns>Response content; null if failed</returns>
    Task<string?> GetFearAndGreedIndex(int limit, bool csvFormat = false);
}