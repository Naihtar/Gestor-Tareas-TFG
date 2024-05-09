using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {
        public CommandViewModel LogOutCommand { get; }
        public CommandViewModel EditCommand { get; }

        public UserProfileViewModel(AppUser user, NavigationService nav) : base(user, nav) {
            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
        }
        private void LogOut(object obj) {
            _user?.Dispose();
            _navigationService.NavigateToLogin();
        }
        private void EditProfile(object obj) {
            _navigationService.NavigateTo("ProfileEdit", _user, _navigationService);
        }

        protected override Task SaveChangesAsyncWrapper() {
            throw new NotImplementedException();
        }
    }
}
