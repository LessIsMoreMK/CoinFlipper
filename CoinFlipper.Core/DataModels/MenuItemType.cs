namespace CoinFlipper.Core
{
    /// <summary>
    /// The types of items for a menu item
    /// </summary>
    public enum MenuItemType
    {
        /// <summary>
        /// No icon
        /// </summary>
        None = 0,

        /// <summary>
        /// Show the menu text and an icon
        /// </summary>
        TextAndIcon = 1,

        /// <summary>
        /// Shows a simple divider between the menu items
        /// </summary>
        Divider = 2,

        /// <summary>
        /// Shows a menu text as header
        /// </summary>
        Header = 3,
    }
}
