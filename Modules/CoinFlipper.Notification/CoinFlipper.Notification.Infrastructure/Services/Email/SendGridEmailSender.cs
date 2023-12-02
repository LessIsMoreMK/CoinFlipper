using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.Notification.Application.ValueObjects;
using CoinFlipper.ServiceDefaults.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CoinFlipper.Notification.Infrastructure.Services.Email;

public class SendGridEmailSender(IConfiguration configuration, ILogger<SendGridEmailSender> logger) : IEmailSender
{
    #region Setup

    private SendGridOptions SendGridOptions { get; } = ((IConfiguration) configuration).GetOptions<SendGridOptions>("email");

    #endregion
    
    #region Methods

    public async Task<SendEmailResponse> SendEmailAsync(EmailDetails emailDetails)
    {
        //When not specified earlier email is send to and from application email.
        var from = new EmailAddress(
            string.IsNullOrWhiteSpace(emailDetails.FromEmail) ? SendGridOptions.FromEmail : emailDetails.FromEmail, 
            string.IsNullOrWhiteSpace(emailDetails.FromName) ? SendGridOptions.FromName : emailDetails.FromName);
        var to = new EmailAddress(
            string.IsNullOrWhiteSpace(emailDetails.ToEmail) ? SendGridOptions.FromEmail : emailDetails.ToEmail, 
            string.IsNullOrWhiteSpace(emailDetails.ToName) ? SendGridOptions.FromName : emailDetails.ToName);
        
        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            emailDetails.Subject, 
            emailDetails.IsHTML ? null : emailDetails.Content,
            emailDetails.IsHTML ? emailDetails.Content : null);
        
        var client = new SendGridClient(SendGridOptions.SendGridApiKey);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            return new SendEmailResponse();

        try
        {
            var reason = await response.Body.ReadAsStringAsync();
            var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(reason);

            var errorResponse = new SendEmailResponse
            {
                Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
            };

            if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact support." });

            return errorResponse;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Exception thrown while Sending email.");
            
            return new SendEmailResponse
            {
                Errors = new List<string>(new[] { "Unknown error occurred" })
            };
        }
    }
    
    #endregion
}