namespace CoinFlipper.Tracer.Application.Clients;

/// <summary>
/// Client for communication with CoinGecko 
/// Documentation: https://www.coingecko.com/en/api/documentation
/// </summary>
public interface ICoinGeckoClient
{
    #region General
    
    /// <summary>
    /// Test call for checking connection
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> TestCall();
    
    /// <summary>
    /// List all supported coins id, name and symbol
    /// Cache / Update Frequency: every 5 minutes
    /// </summary>
    /// <param name="includePlatformAddresses">Include platform contract addresses</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinList(bool includePlatformAddresses = false);
    
    /// <summary>
    /// Get coin info from contract address
    /// Cache / Update Frequency: every 60 seconds
    /// </summary>
    /// <param name="id">CoinGecko coin id</param>
    /// <param name="contractAddress">Contract address of the coin</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinInfoFromContractAddress(string id, string contractAddress);
    
    /// <summary>
    /// List all categories
    /// Cache / Update Frequency: every 5 minutes
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCategoriesList();
    
    /// <summary>
    /// List all categories with market data
    /// Cache / Update Frequency: every 5 minutes
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCategoriesListWithData();
    
    /// <summary>
    /// Get cryptocurrency global data
    /// Cache / Update Frequency: every 10 minutes
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetGlobalCryptoData();
    
    /// <summary>
    /// Get Top 100 Cryptocurrency Global Decentralized Finance(defi) data
    /// Cache / Update Frequency: every 60 minutes
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetGlobalDefiData();
    
    /// <summary>
    /// Get Top 7 searched coins on CoinGecko in the last 24 hours
    /// Cache / Update Frequency: every 60 minutes
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetTop7TrendingCoins();
    
    /// <summary>
    /// Get list of all asset platforms - blockchain networks 
    /// </summary>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetBlockchainNetworks();
    
    #endregion
    
    #region Price
    
    /// <summary>
    /// Get the current price of coins
    /// </summary>
    /// <param name="ids">CoinGecko coinIds (comma separated)</param>
    /// <param name="marketCap">Include market cap</param>
    /// <param name="volume24h">Include 24h volume</param>
    /// <param name="change24h">Include 24h change</param>
    /// <param name="lastUpdatedAt">Include timestamp of last price update</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinsPrice(string ids, bool marketCap = true, bool volume24h = true, bool change24h = true, bool lastUpdatedAt = true);
    
    /// <summary>
    /// Get the current price of token using contract address
    /// It returns the global average price that is aggregated across all active exchanges on CoinGecko. It does not return the price of a specific network
    /// </summary>
    /// <param name="id">CoinGecko coin id</param>
    /// <param name="contractAddress">Contract address of the coin</param>
    /// <param name="marketCap">Include market cap</param>
    /// <param name="volume24h">Include 24h volume</param>
    /// <param name="change24h">Include 24h change</param>
    /// <param name="lastUpdatedAt">Include timestamp of last price update</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetTokenPrice(string id, string contractAddress, bool marketCap = true, bool volume24h = true, bool change24h = true, bool lastUpdatedAt = true);
    
    /// <summary>
    /// Get historical market data include price, market cap, and 24h volume (granularity auto)
    /// Data granularity is automatic (1day = 5m; 2-90days = 1h; >90days = 1day)
    /// Cache based on days range: (1day = 30s; 2-90 days = 30m; >90days = 12h)
    /// </summary>
    /// <param name="id">CoinGecko coin id</param>
    /// <param name="contractAddress">Contract address of the coin</param>
    /// <param name="days">Range in days</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinPriceFromContractAddress(string id, string contractAddress, int days);
    
    /// <summary>
    /// Get coin candle's body
    /// Data granularity is automatic (1-2days = 30m; 3-30days = 4h; >31days = 4days)
    /// Cache / Update Frequency: every 30 minutes
    /// </summary>
    /// <param name="id">CoinGecko coin id</param>
    /// <param name="days">Range in days</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinOHLC(string id, int days);
    
    /// <summary>
    /// Get the price of coin in history
    /// </summary>
    /// <param name="id">CoinGecko coin id</param>
    /// <param name="from">Timestamp from</param>
    /// <param name="to">Timestamp to</param>
    /// <returns>Response content; null if failed</returns>
    public Task<string?> GetCoinPriceHistory(string id, long from, long to);

    #endregion
}