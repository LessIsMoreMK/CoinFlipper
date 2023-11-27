using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Application.Request;

public class RequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    public async Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default) 
        where TRequest : class, IRequest
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
        return await handler.HandleAsync(request, cancellationToken);
    }
}