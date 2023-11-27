namespace CoinFlipper.ServiceDefaults.Application.Request;

public interface IRequestDispatcher
{
    Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class, IRequest;
}