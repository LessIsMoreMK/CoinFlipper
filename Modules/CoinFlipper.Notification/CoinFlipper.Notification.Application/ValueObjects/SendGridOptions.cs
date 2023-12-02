namespace CoinFlipper.Notification.Application.ValueObjects;

public class SendGridOptions
{
    public string SendGridApiKey { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
}