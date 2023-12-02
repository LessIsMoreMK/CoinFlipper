using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.ValueObjects;

namespace CoinFlipper.Notification.Application.Services.Email;

/// <summary>
/// Sends emails using the <see cref="IEmailSender"/> whit a selected template
/// </summary>
public interface IEmailTemplateSender
{
    /// <summary>
    /// Sends an email with the given details using the General template
    /// </summary>
    /// <param name="emailDetails">The email message details. NOTE: The Content property is ignored and replaced with the template</param>
    /// <param name="title">The title</param>
    /// <param name="content1">The first line contents</param>
    /// <param name="content2">The second line contents</param>
    /// <param name="buttonText">The button text</param>
    /// <param name="buttonUrl">The button URL</param>
    /// <returns></returns>
    Task<SendEmailResponse> SendGeneralTemplateEmailAsync(EmailDetails emailDetails, string title, string content1, string content2, string buttonText, string buttonUrl);
}