using System.Windows.Controls;
using TFG.ViewModels;
using TFG.Views.Pages;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    public class NavigationService : INavigationService {
        private readonly Frame _frame;

        public NavigationService(Frame frame) {
            _frame = frame;
        }

        public void NavigateToLogin() {
            _frame.Navigate(new LoginPage(_frame));
        }


        public void NavigateTo(string route, AppUser user, INavigationService nav) {

            switch (route) {

                case "Workspace":
                    _frame.Navigate(new WorkSpacePage(user, this));
                    break;
                case "Profile":
                    _frame.Navigate(new UserProfilePage(user, this));
                    break;
                case "ProfileEdit":
                    _frame.Navigate(new UserProfileEditPage(user, this));
                    break;
            }
        }
        public void GoBack() {
            if (_frame.CanGoBack) {
                _frame.GoBack();
            }
        }

    }
}

