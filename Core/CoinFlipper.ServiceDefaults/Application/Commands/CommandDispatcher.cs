using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Application.Commands;

internal sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
        await handler.HandleAsync(command, cancellationToken);
    }
}