using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TFG.Services.AuthentificationServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    class LoginViewModel : BaseViewModel {

        private readonly AuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private User? _user;

        public string Username { get; set; }
        public string Password { get; set; }
        private string? _errorMessage;
        public string? ErrorMessage {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public CommandViewModel LoginCommand { get; }
        public LoginViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            _authenticationService = new AuthenticationService();

            Username = string.Empty;
            Password = string.Empty;

            LoginCommand = new CommandViewModel(async (object obj) => await LoginAsync(), CanLogin);

        }
        private bool CanLogin(object obj) {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        // Método para autenticar al usuario
        private async Task LoginAsync() {
            string username = Username;
            string password = Password;

            bool isAuthenticated = await _authenticationService.AuthenticateUserAsync(username, password);
            if (!isAuthenticated) {
                ErrorMessage = "Usuario o contraseña incorrecto/a.";
                return;
            }
            _user = await _authenticationService.GetUserByUsernameAsync(username);
            _navigationService.NavigateToWorkSpace(_user);
        }
    }
}
