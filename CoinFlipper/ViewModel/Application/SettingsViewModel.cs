using CoinFlipper.Core;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinFlipper
{
    /// <summary>
    /// The settings state as a view model
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current users name
        /// </summary>
        public TextEntryViewModel Name { get; set; }

        /// <summary>
        /// The current users username
        /// </summary>
        public TextEntryViewModel Username { get; set; }

        /// <summary>
        /// The current users password
        /// </summary>
        public PasswordEntryViewModel Password { get; set; }

        /// <summary>
        /// The current users email
        /// </summary>
        public TextEntryViewModel Email { get; set; }

        /// <summary>
        /// The text for the logout button
        /// </summary>
        public string LogoutButtonText { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open the settings menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// The command to logout of the application
        /// </summary>
        public ICommand LogoutCommand { get; set; }

        /// <summary>
        /// The command to clear the users data from the view model
        /// </summary>
        public ICommand ClearUserDataCommand { get; set; }

        /// <summary>
        /// Loads the settings data from the client data store
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Saves the username name to the server
        /// </summary>
        public ICommand SaveUsernameCommand { get; set; }

        /// <summary>
        /// Saves the current name to the server
        /// </summary>
        public ICommand SaveNameCommand { get; set; }

        /// <summary>
        /// Saves the current email to the server
        /// </summary>
        public ICommand SaveEmailCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {
            // Create commands
            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);
            LogoutCommand = new RelayCommand(Logout);
            ClearUserDataCommand = new RelayCommand(ClearUserData);
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            SaveUsernameCommand = new RelayCommand(async () => await SaveUsernameAsync());
            SaveNameCommand = new RelayCommand(async () => await SaveNameAsync());
            SaveEmailCommand = new RelayCommand(async () => await SaveEmailAsync());

            //TODO: get from localization
            LogoutButtonText = "Logout";
        }

        #endregion

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        public void Close()
        {
            // Close setting menu
            CoinFlipper.DI.ViewModelApplication.SettingsMenuVisible = false;
        }

        /// <summary>
        /// Open the settings menu
        /// </summary>
        public void Open()
        {
            // Open the setting menu
            CoinFlipper.DI.ViewModelApplication.SettingsMenuVisible = true;
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        public void Logout()
        {
            // TODO: confirm logout

            // TODO: Clear any user data/cache

            // Clean all application level view model that contain any information about current user
            ClearUserData();

            // Go to login page
            CoinFlipper.DI.ViewModelApplication.GoToPage(ApplicationPage.Login);
        }

        /// <summary>
        /// Clear any data specific to the current user
        /// </summary>
        public void ClearUserData()
        {
            // Clear all view models containing the user info
            Name = null;
            Username = null;
            Password = null;
            Email = null;
        }

        /// <summary>
        /// Sets the settings view model properties based on the data in the client data store
        /// </summary>
        public async Task LoadAsync()
        {
            // Get the stored credentials
            var storedCredentials = await CoinFlipper.DI.ClientDataStore.GetLoginCredentialsAsync();

            Name = new TextEntryViewModel
            {
                Label = "Name",
                OriginalText = $"{storedCredentials?.FirstName} {storedCredentials?.LastName}",
                CommitAction = SaveNameAsync
            };
            Username = new TextEntryViewModel 
            { 
                Label = "Username", 
                OriginalText = storedCredentials?.Username,
                CommitAction = SaveUsernameAsync
            };
            Password = new PasswordEntryViewModel 
            { 
                Label = "Password", 
                FakePassword = "********",
                CommitAction = SavePasswordAsync
            };
            Email = new TextEntryViewModel 
            { 
                Label = "Email", 
                OriginalText = storedCredentials?.Email,
                CommitAction = SaveEmailAsync
            };
        }

        /// <summary>
        /// Saves the new Username to the server
        /// </summary>
        /// <returns>Return true if successful, false otherwise</returns>
        public async Task<bool> SaveUsernameAsync()
        {
            // Update with server
            await Task.Delay(3000);

            // Return fail
            return false;
        }

        /// <summary>
        /// Saves the new Name to the server
        /// </summary>
        /// <returns>Return true if successful, false otherwise</returns>
        public async Task<bool> SaveNameAsync()
        {
            // Update with server
            await Task.Delay(3000);

            // Return success
            return true;
        }

        /// <summary>
        /// Saves the new Email to the server
        /// </summary>
        /// <returns>Return true if successful, false otherwise</returns>
        public async Task<bool> SaveEmailAsync()
        {
            // Update with server
            await Task.Delay(3000);

            // Return fail
            return false;
        }

        /// <summary>
        /// Saves the new Password to the server
        /// </summary>
        /// <returns>Return true if successful, false otherwise</returns>
        public async Task<bool> SavePasswordAsync()
        {
            // Update with server
            await Task.Delay(3000);

            // Return fail
            return false;
        }

    }
}
