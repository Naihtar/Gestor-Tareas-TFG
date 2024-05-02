using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    class UserProfileViewModel : BaseViewModel {
        private readonly User? _user;
        private DatabaseService _databaseService; // Realizar funciones crud.
        private NavigationService _navigationService;

        public CommandViewModel WorkspaceCommand { get; }
        public CommandViewModel LogOutCommand { get; }
        private Dictionary<string, string>? _userProfileProperties;
        public Dictionary<string, string>? UserProfileProperties {
            get { return _userProfileProperties; }
            set {
                _userProfileProperties = value;
                OnPropertyChanged(nameof(UserProfileProperties));
            }
        }

        public UserProfileViewModel(User user, NavigationService nav) {
            _user = user;
            _databaseService = new DatabaseService();
            _navigationService = nav;
            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            LogOutCommand = new CommandViewModel(LogOut);
            UserData();
        }

        private async void UserData() {
            User u = await _databaseService.GetUserByIdAsync(_user.IdUsuario);

            UserProfileProperties = new Dictionary<string, string> {
                { "Username:", u.AliasUsuario },
                { "Email:", u.EmailUsuario },
                { "Name:", u.NombreUsuario },
                { "Surname:", (u.Apellido1Usuario + " " + u.Apellido2Usuario) },
            };
        }

        private void WorkspaceBack(object obj) {

            _navigationService.GoBack();
        }
        private void LogOut(object obj) {
            _user.Dispose();
            _navigationService.NavigateToLogin();
        }
    }
}
