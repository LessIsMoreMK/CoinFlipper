using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Application.Queries;

internal sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
#pragma warning disable CS8600
        return await ((Task<TResult>) handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
            .Invoke(handler, new object[] {query, cancellationToken}))!;
#pragma warning restore CS8600
    }

    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class, IQuery<TResult>
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }
}