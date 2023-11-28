using System.Text.Json;
using CoinFlipper.ServiceDefaults.Application.Commands;
using CoinFlipper.ServiceDefaults.Attributes;
using CoinFlipper.ServiceDefaults.Options;
using Microsoft.Extensions.Logging;

namespace CoinFlipper.ServiceDefaults.Logging.Decorators;

[Decorator]
internal sealed class CommandHandlerLoggingDecorator<TCommand>(
    ILogger<ICommandHandler<TCommand>> logger,
    ICommandHandler<TCommand> handler
    ) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandJson = JsonSerializer.Serialize(command, JsonOptions.DefaultOptions);

        logger.LogInformation("Handling a command: {Command} {CommandJson}", typeof(TCommand).Name, commandJson);

        try
        {
            await handler.HandleAsync(command, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while handling a command: {Command} {CommandJson}", typeof(TCommand).Name, commandJson);
            throw;
        }
    }
}