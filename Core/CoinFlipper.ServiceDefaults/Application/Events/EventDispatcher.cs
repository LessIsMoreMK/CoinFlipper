using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Application.Events;

internal sealed class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
        foreach (var handler in handlers)
            await handler.HandleAsync(@event, cancellationToken);
    }
}