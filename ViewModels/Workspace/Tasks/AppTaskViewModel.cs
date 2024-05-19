using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.ViewModels.Workspace.Container;

namespace TFG.ViewModels.Workspace.Tasks {
    public class AppTaskViewModel : AppTaskBaseViewModel {

        public CommandViewModel EditTaskAccessCommand { get; }
        public CommandViewModel DeleteTaskCommand { get; }


        public AppTaskViewModel(AppContainer? container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth, AppTask task) : base(task, container, user, navigationService, db, auth) {
            EditTaskAccessCommand = new CommandViewModel(EditTaskAccess);
            DeleteTaskCommand = new CommandViewModel(async (obj) => await DeleteTask());

        }

        private void EditTaskAccess(object obj) {
            _navigationService.NavigateTo("EditTask", _appContainer, _user, _navigationService, _databaseService, _authenticationService, EditableTask);

        }
        private async Task DeleteTask() {
            await _databaseService.DeleteTask(EditableTask.IdTarea);
            _navigationService.NavigateTo("Workspace", _appContainer, _user, _navigationService, _databaseService, _authenticationService);
        }
        protected override Task SaveTaskAsyncWrapper() {
            return Task.CompletedTask;
        }
    }
}
