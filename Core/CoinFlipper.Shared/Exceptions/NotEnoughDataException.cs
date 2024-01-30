namespace CoinFlipper.Shared.Exceptions;

public class NotEnoughDataException : Exception
{
    public NotEnoughDataException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}