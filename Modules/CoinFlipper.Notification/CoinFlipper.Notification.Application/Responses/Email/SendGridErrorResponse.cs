namespace CoinFlipper.Notification.Application.Responses.Email;

public class SendGridErrorResponse
{
    public string Message { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string Help { get; set; } = null!;
}