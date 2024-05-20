using System.Windows;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {
    public class LogInViewModel : BaseViewModel {
        //Atributos
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private AppUser? _user;

        //TODO - CreateAccountCommand.
        public CommandViewModel LogInCommand { get; private set; }
        public CommandViewModel SignUpCommand { get; private set; }
        public string Data { get; set; }
        public string Password { get; set; }
        

        public LogInViewModel(INavigationService navigationService, IDatabaseService databaseService, IAuthenticationService auth) {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _authenticationService = auth;
            Data = string.Empty;
            Password = string.Empty;

            LogInCommand = new CommandViewModel(async (obj) => await LoginAsync(), CanLogin);
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
            _navigationService.NavigateTo("Workspace",_user, _navigationService, _databaseService, _authenticationService);
        }
    }
}
