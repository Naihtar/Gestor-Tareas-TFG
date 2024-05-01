using System.Collections.ObjectModel;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {
        private User _user;
        private MainWindowViewModel _mainWindowViewModel;
        private readonly INavigationService _navigationService;

        private readonly DatabaseService _databaseService;

        public CommandViewModel UserProfileCommand { get; private set; }


        public WorkSpaceViewModel(User user, MainWindowViewModel mainWindowViewModel, NavigationService nav) {

            _user = user;
            _mainWindowViewModel = mainWindowViewModel;
            _databaseService = new DatabaseService();
            _navigationService = nav;
            UserProfileCommand = new CommandViewModel(UserProfileAccess);
        }


        private void UserProfileAccess(object obj) {
            _navigationService.NavigateTo("Profile", _user, _mainWindowViewModel,_navigationService);
        }

        //TODO -> REVISAR COMO PARAMETRIZAR LOS NAVIGATION SERVICES SIN USAR CASTING.
        //TODO -> DISEÑAR LA VISTA DE EL USUARIO.
    }
}
