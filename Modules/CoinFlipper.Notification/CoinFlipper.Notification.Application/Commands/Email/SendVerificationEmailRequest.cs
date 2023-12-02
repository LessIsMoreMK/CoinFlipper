using CoinFlipper.ServiceDefaults.Application.Commands;

namespace CoinFlipper.Notification.Application.Commands.Email;

public class SendVerificationEmailRequest : ICommand
{
    /// <summary>
    /// User username
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Email of the username to which verification an email will be send 
    /// </summary>
    public string Email { get; set; }

    public SendVerificationEmailRequest(string displayName, string email)
    {
        DisplayName = displayName;
        Email = email;
    }
}

