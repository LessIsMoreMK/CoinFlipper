namespace CoinFlipper
{
    /// <summary>
    /// The design-time data for a <see cref="TextEntryViewModel"/>
    /// </summary>
    public class TextEntryDesignModel : TextEntryViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static TextEntryDesignModel Instance => new TextEntryDesignModel();

        #endregion

        #region Constructor

        public TextEntryDesignModel()
        {
            Label = "Name";
            OriginalText = "Maciej Kulaszewicz";
            EditedText = "Editing...";
        }

        #endregion
    }
}
