using System.Collections.Generic;

namespace CoinFlipper.Core
{
    /// <summary>
    /// The design-time data for a <see cref="ChatListViewModel"/>
    /// </summary>
    public class ChatListDesignModel : ChatListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatListDesignModel Instance => new ChatListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListDesignModel()
        {
            Items = new List<ChatListItemViewModel>
            {
               new ChatListItemViewModel
               {
                    Initials = "AS",
                    Name = "Alice",
                    Message = "Very important message",
                    ProfilePictureRGB = "3275A8",
                    NewContentAvailable = true,
               },
               new ChatListItemViewModel
               {
                    Initials = "JA",
                    Name = "John",
                    Message = "Send me that photos from yesterday",
                    ProfilePictureRGB = "FF0032",
               },
               new ChatListItemViewModel
               {
                    Initials = "LA",
                    Name = "Lynda",
                    Message = "What about holidays?",
                    ProfilePictureRGB = "F03276",
                    IsSelected = true,
               },
               new ChatListItemViewModel
               {
                    Initials = "AD",
                    Name = "Ann",
                    Message = "Smart and resourceful",
                    ProfilePictureRGB = "DD9823",
               },
               new ChatListItemViewModel
               {
                    Initials = "AS",
                    Name = "Alice",
                    Message = "Very important message",
                    ProfilePictureRGB = "3275A8",
               },
               new ChatListItemViewModel
               {
                    Initials = "JA",
                    Name = "John",
                    Message = "Send me that photos from yesterday",
                    ProfilePictureRGB = "FF0032",
               },
               new ChatListItemViewModel
               {
                    Initials = "LA",
                    Name = "Lynda",
                    Message = "What about holidays?",
                    ProfilePictureRGB = "F03276",
               },
               new ChatListItemViewModel
               {
                    Initials = "AD",
                    Name = "Ann",
                    Message = "Smart and resourceful",
                    ProfilePictureRGB = "DD9823",
               },
            };
        }
        
        #endregion
    }
}
