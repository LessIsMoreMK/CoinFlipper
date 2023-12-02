using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.Notification.Application.ValueObjects;

namespace CoinFlipper.Notification.Infrastructure.Services.Email;

public class ApplicationEmailSender(IEmailTemplateSender emailTemplateSender, IEmailSender emailSender) : IApplicationEmailSender
{
    #region Methods

    public async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
    {
        var emailDetails = new EmailDetails
        {
            IsHTML = true,
            ToEmail = email,
            ToName = displayName,
            Subject = "Verify Your Email - Coin Flipper"
        };
        var name = string.IsNullOrWhiteSpace(displayName) ? displayName : "stranger";
        var content1 = $"Hi, {name}";
        
        return await emailTemplateSender.SendGeneralTemplateEmailAsync(
            emailDetails, 
            "Verify Email",
            content1,
            "Thanks for creating an account.<br>To continue please verify your email.",
            "Verify Email",
            verificationUrl);
    }
    
    public async Task<SendEmailResponse> SendFeedbackEmailAsync(string displayName, string email, string feedbackContent)
    {
        var content = $"User: {displayName} with email: {email} sends feedback: \n{feedbackContent}";
        var emailDetails = new EmailDetails
        {
            IsHTML = false,
            Subject = "Feedback Email",
            Content = content,
            FromName = displayName,
            FromEmail = email
        };
        
        return await emailSender.SendEmailAsync(emailDetails);
    }
    
    #endregion
}