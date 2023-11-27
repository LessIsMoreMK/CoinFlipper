using System.Text.Json;
using CoinFlipper.ServiceDefaults.Application.Events;
using CoinFlipper.ServiceDefaults.Attributes;
using CoinFlipper.ServiceDefaults.Settings;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.ServiceDefaults.Logging;

[Decorator]
internal sealed class EventHandlerLoggingDecorator<TEvent>(
	ILogger<IEventHandler<TEvent>> logger,
	IEventHandler<TEvent> handler
) : IEventHandler<TEvent> where TEvent : class, IEvent
{
	public async Task HandleAsync(TEvent e, CancellationToken cancellationToken = default)
	{
		var eventJson = JsonSerializer.Serialize(e, JsonSettings.DefaultSettings);

		logger.LogInformation("Handling an event: {Event} {EventJson}", typeof(TEvent).Name, eventJson);

		try
		{
			await handler.HandleAsync(e, cancellationToken);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occured while handling an event: {Event} {EventJson}", typeof(TEvent).Name, eventJson);
			throw;
		}
	}
}