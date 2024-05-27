using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Access {
    class SignUpViewModel : BaseViewModel {

        //Dependencias
        private readonly IDatabaseService _databaseService; //Dependencia de los servicios de la gestión de la base de datos
        private readonly IAuthenticationService _authenticationService; //Dependencia de los servicios de autentificación
        private readonly INavigationService _navigationService; //Dependencia de los servicios navegación

        //Atributos
        public AppUser CreateUser { get; set; }
        private string _userEmail;
        public string UserEmail {
            get { return _userEmail; }
            set {
                _userEmail = value;
                OnPropertyChanged(UserEmail);
            }
        }
        private string _userCheckEmail;
        public string UserCheckEmail {
            get { return _userCheckEmail; }
            set {
                _userCheckEmail = value;
                OnPropertyChanged(UserCheckEmail);
            }
        }

        private string _userPassword;
        public string UserPassword {
            get { return _userPassword; }
            set {
                _userPassword = value;
                OnPropertyChanged(UserPassword);
            }
        }

        private string _userCheckPassword;
        public string UserCheckPassword {
            get { return _userCheckPassword; }
            set {
                _userCheckPassword = value;
                OnPropertyChanged(UserCheckPassword);
            }
        }

        //Comandos
        public CommandViewModel GoBackCommand { get; } //Volver atrás
        public CommandViewModel CreateUserCommand { get; } //Crear cuenta

        //Constructor
        public SignUpViewModel(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService) {

            _databaseService = databaseService;
            _navigationService = navigationService;
            _authenticationService = authenticationService;

            _userEmail = string.Empty;
            _userCheckEmail = string.Empty;
            _userPassword = string.Empty;
            _userCheckPassword = string.Empty;

            //Usuario por defecto
            CreateUser = new AppUser {
                AppUserName = string.Empty,
                AppUserSurname1 = string.Empty,
                AppUserSurname2 = string.Empty,
                AppUserUsername = string.Empty,
                AppUserEmail = string.Empty,
                AppUserPassword = string.Empty,
                AppUserAppContainerList = []
            };

            GoBackCommand = new CommandViewModel(GoBack);
            CreateUserCommand = new CommandViewModel(async (obj) => await CreateUserAsync());
        }

        //Métodos

        //Método para volver para atrás
        private void GoBack(object obj) {
            _navigationService.GoBack();
        }

        //Método para crear un usuario.
        private async Task CreateUserAsync() {

            //Comprueba que ambos correos sean iguales 
            if (UserEmail != UserCheckEmail) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ErrorEmailsMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprueba que el email tenga una estructura válida
            if (!IsValidEmail(UserEmail)) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ErrorEmailValidStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprueba que ambas contraseñas coincidan
            if (_userPassword != _userCheckPassword) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ErrorPasswordMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprobamos que el nombre de usuario no existe en la base de datos
            bool username = await CheckUsername(CreateUser.AppUserUsername);
            if (username) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["UserFieldMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprobamos que el correo electrónico no existe en la base de datos
            bool email = await CheckEmail(_userEmail);
            if (email) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["EmailFieldMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            CreateUser.AppUserEmail = _userEmail; //Asignamos el email al usuario
            CreateUser.AppUserPassword = _authenticationService.HashPassword(_userPassword); //Asignamos la contraseña al usuario

            //Comprueba que no quede ningún campo vacío
            if (AreAnyFieldsEmpty()) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["EmptyFielStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Creamos el usuario
            bool success = await _databaseService.CreateUserAsync(CreateUser);
            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
            }
            string? message = ResourceDictionary["SuccessCreateAccountInfoBarStr"] as string; //Mensaje de éxito
            _navigationService.NavigateTo(message); //Retornamos a la pantalla de Log In
        }

        //Metodos auxiliares
        private bool AreAnyFieldsEmpty() {
            return string.IsNullOrEmpty(CreateUser.AppUserUsername) ||
                   string.IsNullOrEmpty(CreateUser.AppUserEmail) ||
                   string.IsNullOrEmpty(CreateUser.AppUserName) ||
                   string.IsNullOrEmpty(CreateUser.AppUserSurname1);
        }

        private async Task<bool> CheckUsername(string userName) {
            //Comprueba si el nombre de usuario existe en la base de datos
            return await _databaseService.CheckUserByUsernameAsync(userName);

        }
        private async Task<bool> CheckEmail(string userEmail) {
            //Comprueba si el correo electrónico existe en la base de datos
            return await _databaseService.CheckUserByEmailAsync(userEmail);

        }
    }
}