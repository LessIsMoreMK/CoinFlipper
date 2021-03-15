using System.Collections.Generic;

namespace CoinFlipper.Core
{
    /// <summary>
    /// A view model for the overview chat message thread list
    /// </summary>
    public class ChatMessageListViewModel : BaseViewModel
    {
        /// <summary>
        /// The chat thread items for the list  
        /// </summary>
        public List<ChatMessageListItemViewModel> Items { get; set; }

    }
}
