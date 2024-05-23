using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;
using System.Windows;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {

        private readonly AppTask _task;
        private readonly AppContainer _container;

        public CommandViewModel LogOutCommand { get; }
        public CommandViewModel EditCommand { get; }
        public CommandViewModel DeleteCommand { get; }

        public UserProfileViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppTask task, AppContainer container) : base(user, nav, db, auth) {
            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            DeleteCommand = new CommandViewModel(DeleteProfile);
            _task = task;
            _container = container;

        }
        private void LogOut(object obj) {
            _user?.Dispose();
            _task?.Dispose();
            _container?.Dispose();
            _navigationService.NavigateTo("LogIn", _databaseService, _authenticationService);
        }
        private void EditProfile(object obj) {
            _navigationService.NavigateTo("ProfileEdit", null, _user, _navigationService, _databaseService, _authenticationService, null);
        }

        private void DeleteProfile(object obj) {
            _navigationService.NavigateTo("DeleteProfile", _container, EditableUser, _navigationService, _databaseService, _authenticationService, _task);
        }

        protected override Task SaveChangesAsyncWrapper() {
            return Task.CompletedTask;
        }
    }
}
