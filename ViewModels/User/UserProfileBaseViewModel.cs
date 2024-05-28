using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {
    public abstract class UserProfileBaseViewModel : BaseViewModel {
        //Dependencias
        protected IDatabaseService _databaseService; //Dependencia de los servicios de la gestión de la base de datos
        protected INavigationService _navigationService; //Dependencia de los servicios navegación

        //Atributos
        protected AppUser _appUser;
        public AppUser AppUserEditable { get; set; }

        //Diccionario para mostrar los datos del usuario
        private Dictionary<string, string>? _userProfileProperties;
        public Dictionary<string, string>? UserProfileProperties {
            get { return _userProfileProperties; }
            set {
                _userProfileProperties = value;
                OnPropertyChanged(nameof(UserProfileProperties));
            }
        }

        //Comandos
        public CommandViewModel GoBackCommand { get; }

        //Constructor
        protected UserProfileBaseViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser) {
            _navigationService = navigationService;
            _appUser = appUser;
            _databaseService = databaseService;

            //Asignamos el usuario parametrizado, en caso de ser null se asigna uno predefinido
            AppUserEditable = appUser ?? new AppUser() {
                AppUserSurname2 = string.Empty,
                AppUserUsername = string.Empty,
                AppUserEmail = string.Empty,
                AppUserPassword = string.Empty,
                AppUserName = string.Empty,
                AppUserSurname1 = string.Empty
            };

            GoBackCommand = new CommandViewModel(GoBack);

            //Cargar los datos del usuario
            AppUserData();
        }

        //Cargar los datos del usuario
        protected async void AppUserData() {
            try {
                //Buscar el usuario en la DB por ID
                AppUser? appUserData = await _databaseService.GetUserByIDAsync(_appUser.AppUserID);

                if (appUserData == null) {
                    SuccessOpen = false;
                    ErrorOpen = true;
                    ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                    StartTimer();
                    return;
                }

                UserProfileProperties = new Dictionary<string, string> {
            { "Username:", appUserData.AppUserUsername },
            { "Email:", appUserData.AppUserEmail },
            { "Name:", appUserData.AppUserName },
            { "First Surname:", appUserData.AppUserSurname1 },
            { "Second Surname:", appUserData.AppUserSurname2 },
        };
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
            }
        }

        protected async Task SaveChangesAsync() {
            // Actualiza el usuario en la base de datos

            bool success = await _databaseService.UpdateUserAsync(AppUserEditable);

            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            // Actualiza las propiedades del perfil de usuario
            AppUserData();

            // Volver a la vista de perfil, mostrando los datos modificados.
            string? msgChanges = ResourceDictionary["SuccessProfileChangeInfoBarStr"] as string; //Mensaje de inicio de sesión exitoso
            _navigationService.NavigateTo("Profile", appUser: AppUserEditable, appContainer: null, appTask: null, data: msgChanges);
        }
        protected abstract Task SaveChangesAsyncWrapper(); //Guardar los cambios en la base de datos
        protected void GoBack(object obj) {
            _navigationService.GoBack();
        }
    }
}