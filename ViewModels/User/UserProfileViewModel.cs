using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;
using System.Windows;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {
        public CommandViewModel LogOutCommand { get; }
        public CommandViewModel EditCommand { get; }
        public CommandViewModel DeleteCommand { get; }

        public UserProfileViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) : base(user, nav, db, auth) {
            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            DeleteCommand = new CommandViewModel(DeleteProfile);

        }
        private void LogOut(object obj) {
            _user?.Dispose();
            _navigationService.NavigateTo("LogIn", _databaseService, _authenticationService);
        }
        private void EditProfile(object obj) {
            _navigationService.NavigateTo("ProfileEdit", _user, _navigationService, _databaseService, _authenticationService);
        }

        private void DeleteProfile(object obj) {
            _navigationService.NavigateTo("DeleteProfile", EditableUser, _navigationService, _databaseService, _authenticationService);
        }

        protected override Task SaveChangesAsyncWrapper() {
            throw new NotImplementedException();
        }
    }
}
