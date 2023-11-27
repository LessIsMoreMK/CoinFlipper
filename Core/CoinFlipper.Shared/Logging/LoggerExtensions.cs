using Microsoft.Extensions.Logging;

namespace CoinFlipper.Shared.Logging;

public static class LoggerExtensions
{
    /// <summary>
    /// When http response ends with !IsSuccessStatusCode logs statusCode, reason and source
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="response"></param>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    /// <returns>true if IsSuccessStatusCode; false otherwise</returns>
    public static async Task<bool> LogHttpResponseError(this ILogger logger, HttpResponseMessage response, string className, string methodName)
    {
        try
        {
            if (response.IsSuccessStatusCode)
                return true;
            
            var statusCode = response.StatusCode;
            var reason = await response.Content.ReadAsStringAsync();
            
            logger.LogError("{className}>{methodName}: StatusCode: {statusCode} " +
                            "Reason: {reason}", className, methodName, statusCode, reason);

            return false;
        
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while reading http response result");
            return false;
        }
    }
}