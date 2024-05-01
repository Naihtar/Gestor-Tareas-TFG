using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {

    public interface INavigationService {
        void NavigateToLogin(MainWindowViewModel mainWindowViewModel);
        void NavigateTo(string route, User user, MainWindowViewModel mainWindowViewModel, INavigationService nav);
        void GoBack();
    }

}
