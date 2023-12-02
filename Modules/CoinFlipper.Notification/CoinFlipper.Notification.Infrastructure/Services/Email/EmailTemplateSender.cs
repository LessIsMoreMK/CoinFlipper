using System.Reflection;
using System.Text;
using CoinFlipper.Notification.Application.Responses.Email;
using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.Notification.Application.ValueObjects;

namespace CoinFlipper.Notification.Infrastructure.Services.Email;

public class EmailTemplateSender(IEmailSender emailSender) : IEmailTemplateSender
{
    #region Methods
    
    public async Task<SendEmailResponse> SendGeneralTemplateEmailAsync(EmailDetails emailDetails, string title, string content1, string content2, string buttonText, string buttonUrl)
    {
        string templateText;
       
        using (var reader = new StreamReader(Assembly.GetEntryAssembly()?.GetManifestResourceStream("CoinFlipper.Notification.Infrastructure.EmailTemplates.GeneralTemplate.htm") ?? throw new InvalidOperationException(), Encoding.UTF8))
        {
           templateText = await reader.ReadToEndAsync();
        }

        templateText = templateText.Replace("--Title--", title)
            .Replace("--Content1--", content1)
            .Replace("--Content2--", content2)
            .Replace("--ButtonText--", buttonText)
            .Replace("--ButtonUrl--", buttonUrl);

        emailDetails.Content = templateText;

        return await emailSender.SendEmailAsync(emailDetails);
    }
    
    #endregion
}