namespace CoinFlipper
{
    /// <summary>
    /// The design-time data for a <see cref="SettingsViewModel"/>
    /// </summary>
    public class SettingsDesignModel : SettingsViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static SettingsDesignModel Instance => new SettingsDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsDesignModel()
        {
            FirstName = new TextEntryViewModel { Label = "First Name", OriginalText = "Maciej" };
            LastName = new TextEntryViewModel { Label = "Last Name", OriginalText = "Kulaszewicz" };
            Username = new TextEntryViewModel { Label = "Username", OriginalText = "LessIsMore" };
            Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
            Email = new TextEntryViewModel { Label = "Email", OriginalText = "maciej8kz@gmail.com" };
        }
        
        #endregion
    }
}
