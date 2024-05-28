using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.User {
    internal class UserProfileDeleteViewModel : UserProfileBaseViewModel {

        //Dependencias
        private readonly IAuthenticationService _authenticationService; //Dependencia de los servicios de autentificación

        //Atributos
        private readonly AppContainer _appContainer;
        private readonly AppTask _appTask;

        private string? _emailUser;
        public string? EmailUser {

            get { return _emailUser.ToLower(); }
            set {
                _emailUser = value.ToLower();
                OnPropertyChanged(nameof(EmailUser));
            }
        }

        private string? _passwordUser;
        public string? PasswordUser {
            get { return _passwordUser; }
            set {
                _passwordUser = value;
                OnPropertyChanged(nameof(PasswordUser));
            }
        }

        //Comandos
        public CommandViewModel DeleteProfileCommand { get; }

        //Constructores
        public UserProfileDeleteViewModel(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService, AppUser appUser, AppContainer container, AppTask task) : base(databaseService, navigationService, appUser) {
            _authenticationService = authenticationService;

            EmailUser = string.Empty;
            PasswordUser = string.Empty;
            _appTask = task;
            _appContainer = container;

            DeleteProfileCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper(), CanDelete);
        }

        //Métodos
        protected override async Task SaveChangesAsyncWrapper() {

            //Comprueba si el usuario existe en la base de datos
            AppUser? checkUser = await _databaseService.GetUserByEmailAsync(EmailUser);
            if (checkUser == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            //Comprueba que ambos correos coinciden
            bool email = checkUser.AppUserEmail.Equals(AppUserEditable.AppUserEmail, StringComparison.CurrentCultureIgnoreCase);
            if (!email) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckEmailFieldStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprueba que ambas contraseñas coinciden, la introducida con la del usuario
            bool password = await _databaseService.VerifyPasswordByUserIDAsync(_appUser.AppUserID, PasswordUser);
            if (!password) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckPasswordFieldStr"] as string;
                StartTimer();
                return;
            }

            //Verifica que los datos sean correctos
            bool verifyUser = await _authenticationService.AuthenticateUserAsync(EmailUser, PasswordUser);
            if (!verifyUser) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["AuthenticationDataStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }
            bool success = await _databaseService.DeleteUserAsync(AppUserEditable);

            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            _appUser?.Dispose(); //Elimina los datos en memoria del usuario
            _appTask?.Dispose(); //Elimina los datos en memoria del contenedor, en caso de tener uno seleccionado
            _appContainer?.Dispose(); //Elimina los datos de la tarea, en caso de tener una seleccionada
            string? message = ResourceDictionary["SuccessDeleteAccountInfoBarStr"] as string; //Mensaje de éxito
            _navigationService.NavigateTo(message); //Retornamos a la pantalla de Log Ing
        }

        private bool CanDelete(object obj) {
            //Comprueba que ambos campos en la vista no esten vacíos
            return !string.IsNullOrEmpty(EmailUser) && !string.IsNullOrEmpty(PasswordUser);
        }

    }
}
