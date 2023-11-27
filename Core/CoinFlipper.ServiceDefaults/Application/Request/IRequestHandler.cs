namespace CoinFlipper.ServiceDefaults.Application.Request;

public interface IRequestHandler<in TRequest, TResult> where TRequest : class, IRequest 
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}