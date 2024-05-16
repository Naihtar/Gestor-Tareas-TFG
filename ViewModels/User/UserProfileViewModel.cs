using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {
        public CommandViewModel LogOutCommand { get; }
        public CommandViewModel EditCommand { get; }
        private readonly IDatabaseService _db;
        private readonly IAuthenticationService _authenticationService;

        public UserProfileViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) : base(user, nav, db, auth) {
            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            _db = db;
            _authenticationService = auth;

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
