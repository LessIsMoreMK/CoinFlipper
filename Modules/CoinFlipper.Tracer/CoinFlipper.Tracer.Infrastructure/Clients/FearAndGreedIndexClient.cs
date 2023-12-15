using System.Net.Http.Headers;
using System.Web;
using CoinFlipper.Shared.Logging;
using CoinFlipper.Tracer.Application.Clients;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.Tracer.Infrastructure.Clients;

public class FearAndGreedIndexClient(
    ILogger<FearAndGreedIndexClient> logger, 
    HttpClient httpClient) : IFearAndGreedIndexClient
{
    #region Methods

    public async Task<string?> GetFearAndGreedIndex(int limit, bool csvFormat = false)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["limit"] = limit.ToString();
        if (csvFormat)
            query["format"] = "csv";
        
        var request = new UriBuilder("https://api.alternative.me/fng/")
        {
            Query = query.ToString()
        }.ToString();
        
        
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, request));
        var result = await logger.LogHttpResponseError(response, "FearAndGreedIndexClient", "GetFearAndGreedIndex");
        if (!result)
            return null;
        
        return await response.Content.ReadAsStringAsync();
    }
    
    #endregion
}