using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinFlipper.Core
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating if the register command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Initialize commands
            RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        #endregion

        #region Commands Methods

        /// <summary>
        /// Attempts to register a new user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/>passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task RegisterAsync(object parameter)
        {
            IoC.Application.GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }

        /// <summary>
        /// Takes the user to the login page
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            await RunCommand(() => this.RegisterIsRunning, async () =>
            {
                await Task.Delay(200);

                // TODO: server...
                IoC.Settings.Name = new TextEntryViewModel { Label = "Name", OriginalText = $"Maciej Kulaszewiccz { DateTime.Now.ToLocalTime() }" };
                IoC.Settings.Username = new TextEntryViewModel { Label = "Username", OriginalText = "Lessi" };
                IoC.Settings.Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
                IoC.Settings.Email = new TextEntryViewModel { Label = "Email", OriginalText = "maciej8kz@gmail.com" };

                IoC.Application.GoToPage(ApplicationPage.Chat);
            });
        }

        #endregion
    }
}
