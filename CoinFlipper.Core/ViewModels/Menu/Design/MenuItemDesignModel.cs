namespace CoinFlipper.Core
{
    /// <summary>
    /// The design-item data for a <see cref="MenuItemViewModel"/>
    /// </summary>
    public class MenuItemDesignModel : MenuItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MenuItemDesignModel Instance => new MenuItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuItemDesignModel()
        {
            Text = "Hello";
            Icon = IconType.File;
        }

        #endregion
    }
}
