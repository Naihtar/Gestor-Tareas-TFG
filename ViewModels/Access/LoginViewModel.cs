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
        public string Data { get; set; }
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
            Data = string.Empty;
            Password = string.Empty;

            LoginCommand = new CommandViewModel(async (obj) => await LoginAsync(), CanLogin);
        }


        // Metodo activar "AcessButton".
        private bool CanLogin(object obj) {
            return !string.IsNullOrEmpty(Data) && !string.IsNullOrEmpty(Password);
        }

        // Método para autenticar al usuario
        private async Task LoginAsync() {
            string input = Data;
            string password = Password;

            bool isAuthenticated = await _authenticationService.AuthenticateUserAsync(input, password);
            if (!isAuthenticated) {
                ErrorMessage = "Email o contraseña incorrecto/a.";
                return;
            }
            _user = await _authenticationService.GetUserByDataInput(input);
            _navigationService.NavigateTo("Workspace",_user, (NavigationService)_navigationService);
        }
    }
}
