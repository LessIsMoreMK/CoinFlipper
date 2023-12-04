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
        
        var requestUri = new UriBuilder("https://api.alternative.me/fng/")
        {
            Query = query.ToString()
        }.ToString();
        
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var response = await httpClient.SendAsync(request);
        var result = await logger.LogHttpResponseError(response, "FearAndGreedIndexClient", "GetFearAndGreedIndex");
        if (!result)
            return null;
        
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
    
    #endregion
}