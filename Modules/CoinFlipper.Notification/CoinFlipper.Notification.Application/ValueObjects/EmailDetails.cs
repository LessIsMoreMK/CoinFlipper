namespace CoinFlipper.Notification.Application.ValueObjects;

public class EmailDetails
{
    public string FromName { get; set; } = null!;

    public string FromEmail { get; set; } = null!;

    public string ToName { get; set; } = null!;

    public string ToEmail { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool IsHTML { get; set; }
}