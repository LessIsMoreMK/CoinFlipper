namespace CoinFlipper.Shared.Exceptions;

public class RetryException : Exception
{
    public RetryException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}