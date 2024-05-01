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
        private MainWindowViewModel _mainWindowViewModel;
        private User? _user;

        public CommandViewModel LoginCommand { get; private set; }
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

        public LoginViewModel(INavigationService navigationService, MainWindowViewModel mainWindowViewModel) {
            _navigationService = navigationService;
            _authenticationService = new AuthenticationService();
            _mainWindowViewModel = mainWindowViewModel;
            Username = string.Empty;
            Password = string.Empty;

            LoginCommand = new CommandViewModel(LoginAsyncWrapper, CanLogin);
        }

        private async void LoginAsyncWrapper(object obj) {
            await LoginAsync();
        }


        // Metodo activar "AcessButton".
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
            _navigationService.NavigateTo("Workspace",_user, _mainWindowViewModel, (NavigationService)_navigationService);
        }
    }
}
