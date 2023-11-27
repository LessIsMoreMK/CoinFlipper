namespace CoinFlipper.ServiceDefaults.Application.Events;

public interface IEventDispatcher
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent;
}