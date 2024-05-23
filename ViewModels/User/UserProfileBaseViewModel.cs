using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {
    public abstract class UserProfileBaseViewModel : BaseViewModel {
        protected AppUser _user;
        protected IDatabaseService _databaseService;
        protected INavigationService _navigationService;
        protected IAuthenticationService _authenticationService;
        public CommandViewModel GoBackCommand { get; }

        public AppUser EditableUser { get; set; }

        private Dictionary<string, string>? _userProfileProperties;
        public Dictionary<string, string>? UserProfileProperties {
            get { return _userProfileProperties; }
            set {
                _userProfileProperties = value;
                OnPropertyChanged(nameof(UserProfileProperties));
            }
        }

        protected UserProfileBaseViewModel(AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) {
            _navigationService = navigationService;
            _user = user;
            _databaseService = db;
            _authenticationService = auth;
            GoBackCommand = new CommandViewModel(GoBack);
            EditableUser = user ?? new AppUser() {
                AppUserSurname2 = string.Empty,
                AppUserUsername = string.Empty,
                AppUserEmail = string.Empty,
                AppUserPassword = string.Empty,
                AppUserName = string.Empty,
                AppUserSurname1 = string.Empty
            };
            AppUserData();
        }

        protected async void AppUserData() {
            AppUser u = await _databaseService.GetUserByIDAsync(_user.AppUserID);
            if(u == null) {
                ErrorMessage = "Ha ocurrido un error inesperado al mostrar el usuario.";
                return;
            }
            UserProfileProperties = new Dictionary<string, string> {
                { "Username:", u.AppUserUsername },
                { "Email:", u.AppUserEmail },
                { "Name:", u.AppUserName },
                { "First Surname:", u.AppUserSurname1 },
                { "Second Surname:", u.AppUserSurname2 },
            };
        }

        protected void GoBack(object obj) {
            _navigationService.GoBack();
        }


        protected async Task SaveChangesAsync() {
            // Actualiza el usuario en la base de datos
            bool success = await _databaseService.UpdateUserAsync(EditableUser);

            if (!success) {
                ErrorMessage = "Ha ocurrido un error inesperado al actualizar el usuario.";
                return;
            }

            // Actualiza las propiedades del perfil de usuario
            AppUserData();

            // Navega hacia atrás
            _navigationService.NavigateTo("Profile", null, EditableUser, _navigationService, _databaseService, _authenticationService, null);
        }
        protected abstract Task SaveChangesAsyncWrapper();
    }
}