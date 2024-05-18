using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {
        public CommandViewModel LogOutCommand { get; }
        public CommandViewModel EditCommand { get; }

        public UserProfileViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) : base(user, nav, db, auth) {
            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            

        }
        private void LogOut(object obj) {
            _user?.Dispose();
            _navigationService.NavigateTo(_databaseService, _authenticationService);
        }
        private void EditProfile(object obj) {
            _navigationService.NavigateTo("ProfileEdit", _user, _navigationService, _databaseService, _authenticationService);
        }

        protected override Task SaveChangesAsyncWrapper() {
            throw new NotImplementedException();
        }
    }
}
