namespace CoinFlipper.Notification.Application.Responses.Email;

public class SendGridResponse
{
    public List<SendGridErrorResponse> Errors { get; set; } = null!;
}