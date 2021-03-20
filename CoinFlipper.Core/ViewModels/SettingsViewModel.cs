using System.Windows.Input;

namespace CoinFlipper.Core
{
    /// <summary>
    /// The settings state as a view model
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Public Commands

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open the settings menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            // Create commnads
            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);
        }

        #endregion

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        public void Close()
        {
            // Close the setting menu
            IoC.Application.SettingsMenuVisible = false;
        }

        /// <summary>
        /// Open the settings menu
        /// </summary>
        public void Open()
        {
            // Open the setting menu
            IoC.Application.SettingsMenuVisible = true;
        }
    }
}
