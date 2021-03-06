using CoinFlipper.Core;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlipper.Web.Server
{
    /// <summary>
    /// Handles sending templated emails
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);

            // Read the general template from file
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("CoinFlipper.Web.Server.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Content = templateText;

            // Send email
            return await DI.EmailSender.SendEmailAsync(details);
        }
    }
}
