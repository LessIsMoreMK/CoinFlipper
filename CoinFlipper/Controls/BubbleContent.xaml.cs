using System.Windows.Controls;

namespace CoinFlipper
{
    /// <summary>
    /// Interaction logic for BubbleContent.xaml
    /// </summary>
    public partial class BubbleContent : UserControl
    {
        #region Dependency Property
/*
        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BasePage UIViewModel
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value); 
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));*/

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BubbleContent()
        {
            InitializeComponent();
        }

        #endregion
    }
}
