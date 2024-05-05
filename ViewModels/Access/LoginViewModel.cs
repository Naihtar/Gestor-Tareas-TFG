using System.Windows;
using TFG.Services.AuthentificationServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class LoginViewModel : BaseViewModel {
        //Atributos
        private readonly AuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private AppUser? _user;

        //TODO - CreateAccountCommand.
        public CommandViewModel LoginCommand { get; private set; }
        public string Email { get; set; }
        public string Password { get; set; }
        private string? _errorMessage;

        public string? ErrorMessage {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public LoginViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            _authenticationService = new AuthenticationService();
            Email = string.Empty;
            Password = string.Empty;

            LoginCommand = new CommandViewModel(LoginAsyncWrapper, CanLogin);
        }

        private async void LoginAsyncWrapper(object obj) {
            await LoginAsync();
        }


        // Metodo activar "AcessButton".
        private bool CanLogin(object obj) {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        // Método para autenticar al usuario
        private async Task LoginAsync() {
            string email = Email;
            string password = Password;

            bool isAuthenticated = await _authenticationService.AuthenticateUserAsync(email, password);
            if (!isAuthenticated) {
                ErrorMessage = "Email o contraseña incorrecto/a.";
                return;
            }
            _user = await _authenticationService.GetUserByEmailAsync(email);
            _navigationService.NavigateTo("Workspace",_user, (NavigationService)_navigationService);
        }
    }
}
