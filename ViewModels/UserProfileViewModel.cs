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
        private readonly User _user;
        private MainWindowViewModel _mainWindowViewModel;
        private DatabaseService _databaseService; // Realizar funciones crud.
        private NavigationService _navigationService;

        public Dictionary<string, string> UserProfileProperties { get; set; }
        public CommandViewModel WorkspaceCommand { get; private set; }


        public UserProfileViewModel(User user, MainWindowViewModel mainWindowViewModel, NavigationService nav) {

            _user = user;
            _mainWindowViewModel = mainWindowViewModel;
            _databaseService = new DatabaseService();
            _navigationService = nav;
            WorkspaceCommand = new CommandViewModel(WorkspaceBack);

            UserProfileProperties = new Dictionary<string, string> {
                { "UsernameProfile", _user.AliasUsuario },
                { "EmailProfile", _user.EmailUsuario },
                { "NameProfile", _user.NombreUsuario },
                { "Surname1Profile", _user.Apellido1Usuario },
                { "Surname2Profile", _user.Apellido2Usuario }
            };

        }

        private void WorkspaceBack(object obj) {
            System.Windows.MessageBox.Show("El botón ha sido pulsado");

            _navigationService.GoBack();
        }
        //public string? UsernameProfile { get; set; }
        //public string? EmailProfile { get; set; }
        //public string? NameProfile { get; set; }
        //public string? Surname1Profile { get; set; }
        //public string? Surname2Profile { get; set; }


        //public void UserProfileData() {

        //    UsernameProfile = _user.AliasUsuario;
        //    EmailProfile = _user.EmailUsuario;
        //    NameProfile = _user.NombreUsuario;
        //    Surname1Profile = _user.Apellido1Usuario;
        //    Surname2Profile = _user.Apellido2Usuario;

        //}
    }
}
