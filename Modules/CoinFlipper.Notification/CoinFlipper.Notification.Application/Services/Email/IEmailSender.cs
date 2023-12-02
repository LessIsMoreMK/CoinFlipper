using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.ValueObjects;

namespace CoinFlipper.Notification.Application.Services.Email;

/// <summary>
/// A service that handles sending emails on behalf of the caller
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email message with the given information
    /// </summary>
    /// <param name="emailDetails">The details about the email to send</param>
    /// <returns>SendEmailResponse</returns>
    Task<SendEmailResponse> SendEmailAsync(EmailDetails emailDetails);
}