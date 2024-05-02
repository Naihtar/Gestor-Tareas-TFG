using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {

    public interface INavigationService {
        void NavigateToLogin();
        void NavigateTo(string route, User user, INavigationService nav);
        void GoBack();
    }

}
