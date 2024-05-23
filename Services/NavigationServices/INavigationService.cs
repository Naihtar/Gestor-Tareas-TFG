using System.ComponentModel;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Models;

namespace TFG.Services.NavigationServices {

    public interface INavigationService {
        void NavigateTo(string route ,IDatabaseService db, IAuthenticationService auth);
        void NavigateTo(string route, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth);

        void NavigateTo(string route, AppContainer? container, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth);
        void NavigateTo(string route, AppContainer? container, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth, AppTask?task);
        void NavigateTo(string route, AppContainer? container, AppUser user, INavigationService nav, IDatabaseService databaseService, IAuthenticationService auth, AppTask? task, string status);

        void GoBack();
    }

}
