using System.Collections.Generic;

namespace CoinFlipper.Web.Server
{
    /// <summary>
    /// A response to a SendGrid SendMessage call
    /// </summary>
    public class SendGridResponse
    {
        /// <summary>
        /// Any error form a response
        /// </summary>
        public List<SendGridResponseError> Errors { get; set; }
    }
}
