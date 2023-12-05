using CoinFlipper.ServiceDefaults.Application.Commands;
using Microsoft.AspNetCore.Http;

namespace CoinFlipper.Notification.Application.Commands.Email;

public class SendFeedbackEmailRequest : ICommand
{
    /// <summary>
    /// User username
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Email of the author
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Content of the provided feedback
    /// </summary>
    public string FeedbackContent { get; set; }

    public SendFeedbackEmailRequest(string displayName, string email, string feedbackContent)
    {
        DisplayName = displayName;
        Email = email;
        FeedbackContent = feedbackContent;
    }
}