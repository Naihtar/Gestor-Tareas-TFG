using TFG.Services.AuthentificationServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;
using TFG.Services;

namespace TFG.ViewModels {
    public class LogInViewModel : BaseViewModel {

        //Dependencias
        private readonly IAuthenticationService _authenticationService; //Dependencia de los servicios de autentificación
        private readonly INavigationService _navigationService; //Dependencia de los servicios navegación
        private readonly ILocalizationService _localizationService; //Dependencia de los servicios de traducción de la app

        //Atributos
        private AppUser? _appUser;
        private string? _data; //Corresponde al correo electrónico o nombre de usuario
        public string? Data {

            get { return _data.ToLower(); }
            set {
                _data = value.ToLower();
                OnPropertyChanged(Data); //Notifica al programa que la variable Data ha sido modificada
            }
        }

        private string? _password; //Corresponde a la contraseña del usuario
        public string? Password {
            get { return _password; }
            set {
                _password = value; OnPropertyChanged(Password); //Notifica al programa que la variable Data ha sido modificada
            }
        }

        private readonly string _actualLanguage;

        //Comandos
        public CommandViewModel LogInCommand { get; private set; } //Comando para acceder a tu cuenta
        public CommandViewModel SignUpCommand { get; private set; } //Comando para crear una cuenta
        public CommandViewModel ChangeLanguageCommand { get; private set; } //Cambiar idioma

        //Constructor
        public LogInViewModel(IAuthenticationService authenticationService, INavigationService navigationService, ILocalizationService localizationService, string? successMessage) {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            _localizationService = localizationService;

            Data = string.Empty;
            Password = string.Empty;
            Message = successMessage;
            _actualLanguage = _localizationService.GetLanguage() ?? "es-ES";

            LogInCommand = new CommandViewModel(async (obj) => await LoginAsync(), CanLogin);
            SignUpCommand = new CommandViewModel(SignUpAccess);
            ChangeLanguageCommand = new CommandViewModel(ChangeLanguage);

            DisableLanguageBtns(_actualLanguage);
            if (Message != null) {
                //Muestra mensaje informativo tras completar una acción, como por ejemplo cerrar la sesión.
                ShowSuccessMessage(Message);
            }
        }

        //Métodos:

        // Método activar "AcessButton".
        private bool CanLogin(object obj) {
            //Verifica que tanto la contraseña como el nombre de usuario/correo electrónico no sean campos vacíos
            return !string.IsNullOrEmpty(Data) && !string.IsNullOrEmpty(Password);
        }

        // Método para autenticar al usuario
        private async Task LoginAsync() {
            //Comprueba que el usuario existe en la base de datos
            bool isAuthenticated = await _authenticationService.AuthenticateUserAsync(input: Data, password: Password);
            if (!isAuthenticated) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["AuthenticationDataStr"] as string; //Mensaje de alerta
                StartTimer();
                return;
            }

            //Obtiene el usuario através del correo electrónico o nombre de usuario
            _appUser = await _authenticationService.GetUserByDataInputAsync(input: Data);
            SuccessOpen = false;
            if (_appUser == null) {
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            string? msgLogIn = ResourceDictionary["SuccessAccessInfoBarStr"] as string; //Mensaje de inicio de sesión exitoso
            _navigationService.NavigateTo(appUser: _appUser, appContainer: null, msgLogIn); // Carga la vista de los espacios de trabajo
        }

        private void SignUpAccess(object obj) {
            _navigationService.NavigateTo(); //Carga la vista de creación de cuentas
        }

        private void ChangeLanguage(object obj) {

            string? lang = obj as string;

            lang ??= "es-ES"; //En caso de que status sea "null" se agregara automáticamente a "es-ES"

            DisableLanguageBtns(lang);

            _localizationService.SetLanguage(lang);

            string? msg = ResourceDictionary["SuccessLanguageChangeInfoBarStr"] as string;

            _navigationService.NavigateTo(msg);
        }
    }
}
