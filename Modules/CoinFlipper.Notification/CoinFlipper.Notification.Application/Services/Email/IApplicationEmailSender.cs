using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.ValueObjects;

namespace CoinFlipper.Notification.Application.Services.Email;

/// <summary>
/// Sends emails using directly <see cref="IEmailSender"/> or through adding template in <see cref="IEmailTemplateSender"/>
/// </summary>
public interface IApplicationEmailSender
{
    /// <summary>
    /// Sends an verification email
    /// </summary>
    /// <param name="displayName">User username</param>
    /// <param name="email">User email</param>
    /// <param name="verificationUrl">Url link for verificaion</param>
    /// <returns></returns>
    public Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl);

    /// <summary>
    /// Sends an feedback email with the given details
    /// </summary>
    /// <param name="displayName">User username</param>
    /// <param name="email">User email</param>
    /// <param name="feedbackContent">Message of the feedback</param>
    /// <returns></returns>
    public Task<SendEmailResponse> SendFeedbackEmailAsync(string displayName, string email, string feedbackContent);
}