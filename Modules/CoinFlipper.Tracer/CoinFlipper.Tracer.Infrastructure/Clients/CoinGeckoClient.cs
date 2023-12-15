using System.Net.Http.Headers;
using System.Web;
using CoinFlipper.Shared.Logging;
using CoinFlipper.Tracer.Application.Clients;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Infrastructure.Clients;

public class CoinGeckoClient(
    ILogger<CoinGeckoClient> logger, 
    HttpClient httpClient
    ) : ICoinGeckoClient
{
    private const string BaseUrl = "https://api.coingecko.com/api/v3/";
    
    #region General
    
    public async Task<string?> TestCall()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "ping");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "TestCall");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCoinList(bool includePlatformAddresses = false)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (includePlatformAddresses) 
            query["include_platform"] = "true";
        
        var request = new UriBuilder(BaseUrl + "coins/list")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCoinList");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCoinInfoFromContractAddress(string id, string contractAddress)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = id;
        query["contract_address"] = contractAddress;
        
        var request = new UriBuilder(BaseUrl + $"coins/{id}/contract/{contractAddress}")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCoinInfoFromContractAddress");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCategoriesList()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "coins/categories/list");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCategoriesList");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCategoriesListWithData()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "/coins/categories");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCategoriesListWithData");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetGlobalCryptoData()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "global");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetGlobalCryptoData");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetGlobalDefiData()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "global/decentralized_finance_defi");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetGlobalDefiData");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetTop7TrendingCoins()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "search/trending");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetTop7TrendingCoins");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetBlockchainNetworks()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "asset_platforms");
        
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetBlockchainNetworks");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }
    
    #endregion
    
    #region Price

    public async Task<string?> GetCoinsPrice(string ids, bool marketCap = true, bool volume24h = true, bool change24h = true, bool lastUpdatedAt = true)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["ids"] = ids;
        query["vs_currencies"] = "usd";
        query["include_market_cap"] = marketCap.ToString().ToLower();
        query["include_24hr_vol"] = volume24h.ToString().ToLower();
        query["include_24hr_change"] = change24h.ToString().ToLower();
        query["include_last_updated_at"] = lastUpdatedAt.ToString().ToLower();
        query["precision"] = "full";
        
        var request = new UriBuilder(BaseUrl + "simple/price")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCoinsCurrentPrice");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetTokenPrice(string id, string contractAddress, bool marketCap = true, bool volume24h = true, bool change24h = true, bool lastUpdatedAt = true)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = id;
        query["contract_addresses"] = contractAddress;
        query["vs_currencies"] = "usd";
        query["include_market_cap"] = marketCap.ToString().ToLower();
        query["include_24hr_vol"] = volume24h.ToString().ToLower();
        query["include_24hr_change"] = change24h.ToString().ToLower();
        query["include_last_updated_at"] = lastUpdatedAt.ToString().ToLower();
        query["precision"] = "full";
        
        var request = new UriBuilder(BaseUrl + $"simple/token_price/{id}")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetTokenCurrentPrice");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCoinPriceFromContractAddress(string id, string contractAddress, int days)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = id;
        query["contract_addresses"] = contractAddress;
        query["vs_currency"] = "usd";
        query["days"] = days.ToString();
        query["precision"] = "full";
        
        var request = new UriBuilder(BaseUrl + $"coins/{id}/contract/{contractAddress}/market_chart")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetTokenCurrentPrice");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCoinOHLC(string id, int days)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = id;
        query["vs_currency"] = "usd";
        query["days"] = days.ToString();
        query["precision"] = "full";
        
        var request = new UriBuilder(BaseUrl + $"coins/{id}/ohlc")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetTokenCurrentPrice");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string?> GetCoinPriceHistory(string id, long from, long to)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = id;
        query["vs_currency"] = "usd";
        query["from"] = from.ToString();
        query["to"] = to.ToString();
        query["precision"] = "full";
        
        var request = new UriBuilder(BaseUrl + $"coins/{id}/market_chart/range")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "CoinGeckoClient", "GetCoinPriceHistory");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }

    #endregion
}