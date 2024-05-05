using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {

    public class UserProfileEditPasswordViewModel : UserProfileBaseViewModel {
        public UserProfileEditPasswordViewModel(AppUser? user, NavigationService navigationService) : base(user, navigationService) {
        }
    }
}