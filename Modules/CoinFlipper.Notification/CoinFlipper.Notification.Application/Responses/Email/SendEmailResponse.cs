namespace CoinFlipper.Notification.Application.Responses.Email;

public class SendEmailResponse
{
    public bool Successful => !(Errors?.Count > 0);
    public List<string>? Errors { get; set; }
}