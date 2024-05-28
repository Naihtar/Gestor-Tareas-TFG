using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.ViewModels.Workspace.Container;

namespace TFG.ViewModels.Workspace.Tasks {
    public class AppTaskViewModel : AppTaskBaseViewModel {

        //Comandos
        public CommandViewModel EditTaskAccessCommand { get; } //Editar tarea
        public CommandViewModel DeleteTaskCommand { get; } //Borrar tarea
        public CommandViewModel WorkspaceBackCommand { get; }


        //Constructor
        public AppTaskViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask appTask) : base(databaseService, navigationService, appUser, appContainer, appTask) {
            EditTaskAccessCommand = new CommandViewModel(EditTaskAccess);
            DeleteTaskCommand = new CommandViewModel(async (obj) => await DeleteTask());
            WorkspaceBackCommand = new CommandViewModel(WorkspaceBack);

        }

        //Ir la edición de la tarea
        private void EditTaskAccess(object obj) {
            _navigationService.NavigateTo("TaskEdit", appUser: _appUser, appContainer: _appContainer, appTask: AppTaskEditable, null);

        }

        //Eliminar tarea
        private async Task DeleteTask() {
            bool success = await _databaseService.DeleteTaskAsync(AppTaskEditable.AppTaskID);
            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            AppTaskEditable?.Dispose();
            _appTask?.Dispose();
            TaskProperties.Clear();


            //Ir al espacio de trabajo, y mostrar el mensaje de éxito
            string? message = ResourceDictionary["SuccessTaskDeleteInfoBarStr"] as string;
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, successMessage: message);
        }

        protected override Task SaveTaskAsyncWrapper() {
            return Task.CompletedTask; //Marcar tarea como completada
        }

        private void WorkspaceBack(object obj) {
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, successMessage: null);
        }
    }
}
