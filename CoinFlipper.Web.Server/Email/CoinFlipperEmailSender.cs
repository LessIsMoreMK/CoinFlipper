using System.Threading.Tasks;
using CoinFlipper.Core;

namespace CoinFlipper.Web.Server
{
    /// <summary>
    /// Handles sending emails specific to the Coin Flipper server
    /// </summary>
    public static class CoinFlipperEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specified user
        /// </summary>
        /// <param name="displayName">The user display name (typically first name)</param>
        /// <param name="email">The user email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to verify their email</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
        {
            return await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Dna.FrameworkDI.Configuration["CoinFlipperSettings:SendEmailFromEmail" ],
                FromName = Dna.FrameworkDI.Configuration["CoinFlipperSettings:SendEmailFromName"],
                ToEmail = email,
                ToName = displayName,
                Subject = "Verify Your Email - Coin Flipper"
            }, "Verify Email",
            $"Hi, {displayName ?? "stranger"},",
            "Thanks for creating an account.<br>To continue please verify your email.",
            "Verify Email",
            verificationUrl);
        }
    }
}
