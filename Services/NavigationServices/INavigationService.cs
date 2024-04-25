using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    
        public interface INavigationService {
            void NavigateToLogin();
            void NavigateToWorkSpace(User user);
        }
    
}
