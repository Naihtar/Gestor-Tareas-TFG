using System.ComponentModel;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {

    public interface INavigationService {
        void NavigateTo(IDatabaseService db, IAuthenticationService auth);
        void NavigateTo(string route, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth);

        void NavigateTo(string route, AppContainer? container, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth);

        void GoBack();
    }

}
