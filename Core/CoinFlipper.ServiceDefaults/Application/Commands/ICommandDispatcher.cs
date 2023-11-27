namespace CoinFlipper.ServiceDefaults.Application.Commands;

public interface ICommandDispatcher
{
    Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand;
}