using System.Text.Json;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.ServiceDefaults.Attributes;
using CoinFlipper.ServiceDefaults.Settings;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.ServiceDefaults.Logging;

[Decorator]
internal sealed class QueryHandlerLoggingDecorator<TQuery, TResult>(
    ILogger<IQueryHandler<TQuery, TResult>> logger,
    IQueryHandler<TQuery, TResult> handler
    ) : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var queryJson = JsonSerializer.Serialize(query, JsonSettings.DefaultSettings);

        try
        {
            logger.LogInformation("Handling a query: {Query} {QueryJson}", typeof(TQuery).Name, queryJson);

            return await handler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while handling a query: {Query} {QueryJson}", typeof(TQuery).Name, queryJson);
            throw;
        }
    }
}