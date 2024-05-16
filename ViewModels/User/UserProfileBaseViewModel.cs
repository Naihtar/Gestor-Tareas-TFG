using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public abstract class UserProfileBaseViewModel : BaseViewModel {
        protected AppUser? _user;
        protected IDatabaseService _databaseService;
        protected INavigationService _navigationService;
        protected IAuthenticationService _authenticationService;
        public CommandViewModel WorkspaceCommand { get; }

        public AppUser EditableUser { get; set; }

        private Dictionary<string, string>? _userProfileProperties;
        public Dictionary<string, string>? UserProfileProperties {
            get { return _userProfileProperties; }
            set {
                _userProfileProperties = value;
                OnPropertyChanged(nameof(UserProfileProperties));
            }
        }

        protected UserProfileBaseViewModel(AppUser? user, INavigationService navigationService,  IDatabaseService db, IAuthenticationService auth) {
            _navigationService = navigationService;
            _user = user;
            _databaseService = db;
            _authenticationService = auth;
            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            EditableUser = user ?? new AppUser() {
                Apellido2Usuario = string.Empty,
                AliasUsuario = string.Empty,
                EmailUsuario = string.Empty,
                PasswordUsuario = string.Empty,
                NombreUsuario = string.Empty,
                Apellido1Usuario = string.Empty
            };
            AppUserData();
        }

        protected async void AppUserData() {
            AppUser u = await _databaseService.GetUserByIdAsync(_user.IdUsuario);

            UserProfileProperties = new Dictionary<string, string> {
                { "Username:", u.AliasUsuario },
                { "Email:", u.EmailUsuario },
                { "Name:", u.NombreUsuario },
                { "First Surname:", u.Apellido1Usuario },
                { "Second Surname:", u.Apellido2Usuario },
            };
        }

        private void WorkspaceBack(object obj) {
            _navigationService.GoBack();
        }


        protected async Task SaveChangesAsync() {
            // Actualiza el usuario en la base de datos
            await _databaseService.UpdateUserAsync(EditableUser);

            // Actualiza las propiedades del perfil de usuario
            AppUserData();

            // Navega hacia atrás
            _navigationService.NavigateTo("Profile", EditableUser, _navigationService, _databaseService, _authenticationService);
        }
        protected abstract Task SaveChangesAsyncWrapper();
    }
}