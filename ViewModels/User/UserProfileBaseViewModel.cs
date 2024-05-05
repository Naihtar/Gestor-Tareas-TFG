using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {

    //TODO -> Reestructurar este programa para ver como organizar las vistas, dejando de forma parcial la parte dinamica.
    public abstract class UserProfileBaseViewModel : BaseViewModel {
        protected AppUser? _user;
        protected DatabaseService _databaseService;
        protected NavigationService _navigationService;
        private Dictionary<string, string>? _userProfileProperties;

        public Dictionary<string, string>? UserProfileProperties {
            get { return _userProfileProperties; }
            set {
                _userProfileProperties = value;
                OnPropertyChanged(nameof(UserProfileProperties));
            }
        }
        public CommandViewModel WorkspaceCommand { get; }


        protected UserProfileBaseViewModel(AppUser? user, NavigationService navigationService) {
            _navigationService = navigationService;
            _user = user;
            _databaseService = new DatabaseService();
            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            UserData();
        }

        protected async void UserData() {
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
    }
}
