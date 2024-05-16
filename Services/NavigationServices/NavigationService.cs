using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Views.Pages;
using TFG.Views.Pages.Workspace.Container;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    public class NavigationService(Frame frame) : INavigationService {
        private readonly Frame _frame = frame;
        public void GoBack() {
            if (_frame.CanGoBack) {
                _frame.GoBack();
            }
        }
        public void NavigateTo(IDatabaseService db, IAuthenticationService auth) {
            _frame.Navigate(new LoginPage(this, db, auth));
        }

        public void NavigateTo(string route, AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) {
            switch (route) {
                case "Workspace":
                    _frame.Navigate(new WorkSpacePage(user, this, db, auth, null));
                    break;
                case "ProfileEdit":
                    _frame.Navigate(new UserProfileEditPage(user, this, db, auth));
                    break;
                case "Profile":
                    _frame.Navigate(new UserProfilePage(user, this, db, auth));
                    break;
                case "ProfilePassword":
                    _frame.Navigate(new UserProfileEditPasswordPage(user, this, db, auth));
                    break;
            }

        }

        public void NavigateTo(string route, AppContainer? container, AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) {

            switch (route) {

                case "Container":
                    _frame.Navigate(new AppContainerPage(container, this, user, db, auth));
                    break;
                case "ContainerEdit":
                    _frame.Navigate(new AppContainerCreateOrEditPage(container, user, this, db, auth));
                    break;
                case "ProfilePassword":
                    _frame.Navigate(new UserProfileEditPasswordPage(user, this, db, auth));
                    break;
                case "Workspace":
                    _frame.Navigate(new WorkSpacePage(user, this, db, auth, container)); // Pasamos el contenedor como parámetro
                    break;
                case "ContainerAdd":
                    _frame.Navigate(new AppContainerCreateOrEditPage(container, user, this, db, auth));
                    break;
            }

        }

    }
}

