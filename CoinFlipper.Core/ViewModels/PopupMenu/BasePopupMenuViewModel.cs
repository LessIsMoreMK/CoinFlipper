namespace CoinFlipper.Core
{
    /// <summary>
    /// A view model for any popup menu
    /// </summary>
    public class BasePopupMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The background color of the bubble in ARGB value
        /// </summary>
        public string BubbleBackground { get; set; }

        /// <summary>
        /// The alignment of the bubble
        /// </summary>
        public ElementHorizontalAlignment ArrowAlignment { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePopupMenuViewModel()
        {
            // Set default values
            BubbleBackground = "DBDBDB";
            ArrowAlignment = ElementHorizontalAlignment.Right;
        }

        #endregion
    }
}
