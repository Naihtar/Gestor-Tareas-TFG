using System.Net.Mail;
using System.Windows;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Access {
    class SignUpViewModel : BaseViewModel {
        public CommandViewModel WorkspaceCommand { get; }
        public CommandViewModel CreateUserCommand { get; }
        private readonly IDatabaseService _databaseService;
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;

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
        private string _userCheckPassword;

        public string UserPassword {
            get { return _userPassword; }
            set {
                _userPassword = value;
                OnPropertyChanged(UserPassword);
            }
        }
        public string UserCheckPassword {
            get { return _userCheckPassword; }
            set {
                _userCheckPassword = value;
                OnPropertyChanged(UserCheckPassword);
            }
        }

        public SignUpViewModel(IDatabaseService db, INavigationService nav, IAuthenticationService auth) {

            _databaseService = db;
            _navigationService = nav;
            _authenticationService = auth;

            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            CreateUserCommand = new CommandViewModel(async (obj) => await CreateUserAsync());

            _userEmail = string.Empty;
            _userCheckEmail = string.Empty;
            _userPassword = string.Empty;
            _userCheckPassword = string.Empty;

            CreateUser = new AppUser {
                AppUserName = string.Empty,
                AppUserSurname1 = string.Empty,
                AppUserSurname2 = string.Empty,
                AppUserUsername = string.Empty,
                AppUserEmail = string.Empty,
                AppUserPassword = string.Empty,
                AppUserAppContainerList = []
            };
        }

        private void WorkspaceBack(object obj) {
            _navigationService.GoBack();
        }

        private async Task CreateUserAsync() {
            if (UserEmail != UserCheckEmail) {
                ErrorMessage = "Los Emails no coinciden.";
                return;
            }

            if (!IsValidEmail(UserEmail)) {
                ErrorMessage = "El email introducido no es válido.";
                return;
            }

            if (_userPassword != _userCheckPassword) {
                ErrorMessage = "Las contraseñas no coinciden.";
                return;
            }
            CreateUser.AppUserEmail = _userEmail;
            CreateUser.AppUserPassword = _authenticationService.HashPassword(_userPassword);
            if (AreAnyFieldsEmpty()) {
                ErrorMessage = "Complete los campos vacios.";
                return;
            }
            bool username = await CheckUsername(CreateUser.AppUserUsername);
            bool email = await CheckEmail(CreateUser.AppUserEmail);

            if (username) {
                ErrorMessage = "El usuario ya esta en uso.";
                return;
            }

            if (email) {
                ErrorMessage = "El correo ya esta en uso.";
                return;
            }


            bool success = await _databaseService.CreateUserAsync(CreateUser);

            if (success) {
                MessageBox.Show("Usuario creado correctamente, regresando a la pantalla de inicio");
                _navigationService.NavigateTo("LogIn", _databaseService, _authenticationService);
            }


        }

        private bool AreAnyFieldsEmpty() {
            return string.IsNullOrEmpty(CreateUser.AppUserUsername) ||
                   string.IsNullOrEmpty(CreateUser.AppUserEmail) ||
                   string.IsNullOrEmpty(CreateUser.AppUserName) ||
                   string.IsNullOrEmpty(CreateUser.AppUserSurname1);
        }

        private async Task<bool> CheckUsername(string userName) {

            return await _databaseService.CheckUserByUsernameAsync(userName);

        }
        private async Task<bool> CheckEmail(string userEmail) {

            return await _databaseService.CheckUserByEmailAsync(userEmail);

        }
    }
}