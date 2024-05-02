using System.Collections.ObjectModel;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {
        private User _user;
        private readonly INavigationService _navigationService;

        private readonly DatabaseService _databaseService;

        public CommandViewModel UserProfileCommand { get; private set; }


        public WorkSpaceViewModel(User user, NavigationService nav) {

            _user = user;
            _databaseService = new DatabaseService();
            _navigationService = nav;
            UserProfileCommand = new CommandViewModel(UserProfileAccess);
        }


        private void UserProfileAccess(object obj) {
            _navigationService.NavigateTo("Profile", _user,_navigationService);
        }
    }
}
