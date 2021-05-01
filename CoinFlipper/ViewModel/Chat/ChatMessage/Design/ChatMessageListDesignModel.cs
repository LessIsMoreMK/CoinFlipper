using System;
using System.Collections.ObjectModel;

namespace CoinFlipper
{
    /// <summary>
    /// The design-time data for a <see cref="ChatMessageListViewModel"/>
    /// </summary>
    public class ChatMessageListDesignModel : ChatMessageListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatMessageListDesignModel Instance => new ChatMessageListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessageListDesignModel()
        {
            DisplayTitle = "Alice";

            Items = new ObservableCollection<ChatMessageListItemViewModel>
            {
               new ChatMessageListItemViewModel
               {
                   SenderName = "Alice",
                   Initials = "AS",
                   Message = "We need to talk. Call me as soon as possible.",
                   ProfilePictureRGB = "3275A8",
                   MessageSentTime = DateTimeOffset.UtcNow,
                   SentByMe = false,
               },
               new ChatMessageListItemViewModel
               {
                   SenderName = "Maciej",
                   Initials = "MK",
                   Message = "I'm on a meeting, can't right now. Should finish before 14",
                   ProfilePictureRGB = "3275A8",
                   MessageSentTime = DateTimeOffset.UtcNow,
                   MessageReadTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(1.3)),
                   SentByMe = true,
               },
               new ChatMessageListItemViewModel
               {
                   SenderName = "Alice",
                   Initials = "AS",
                   Message = "Okay, waiting impatiently",
                   ProfilePictureRGB = "3275A8",
                   MessageSentTime = DateTimeOffset.UtcNow,
                   SentByMe = false,
               },
            };
        }
        
        #endregion
    }
}
