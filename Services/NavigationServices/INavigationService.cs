using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    
        public interface INavigationService {
            void NavigateToLogin(MainWindowViewModel mainWindowViewModel);
            void NavigateToWorkSpace(User user, MainWindowViewModel mainWindowViewModel);
        }
    
}
