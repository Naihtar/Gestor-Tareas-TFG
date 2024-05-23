using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public class AppTaskCreateOrEditViewModel : AppTaskBaseViewModel {
        public CommandViewModel SaveTaskCommand { get; }
        private readonly bool _isCreate;
        private readonly string? _status;

        //public AppTaskCreateOrEditViewModel(AppTask task, AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth)
        //    : base(task, container, user, navigationService, db, auth) {
        //    SaveTaskCommand = new CommandViewModel(async (obj) => await SaveTaskAsyncWrapper());
        //    _isCreate = false;
        //}
        //public AppTaskCreateOrEditViewModel(AppTask? task, AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth, string status)
        //    : base(task, container, user, navigationService, db, auth) {
        //    SaveTaskCommand = new CommandViewModel(async (obj) => await SaveTaskAsyncWrapper());
        //    _status = status;
        //    _isCreate = _appTask == null;
        //}

        public AppTaskCreateOrEditViewModel(AppTask? task, AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth, string status = null)
            : base(task, container, user, navigationService, db, auth) {
            SaveTaskCommand = new CommandViewModel(async (obj) => await SaveTaskAsyncWrapper());
            _status = status;
            _isCreate = _appTask == null;
        }

        private bool CheckFieldName() {
            return string.IsNullOrEmpty(EditableTask.AppTaskTitle);
        }

        protected override async Task SaveTaskAsyncWrapper() {
            // Verificar que el campo de nombre no esté vacío
            bool fieldName = CheckFieldName();

            if (fieldName) {
                ErrorMessage = "Rellene el campo del nombre.";
                return;
            }

            // Verificar si alguna etiqueta excede los 16 caracteres
            var invalidTags = EditableTask.AppTaskTags.Where(tag => tag.Length > 16).ToList();
            if (invalidTags.Count != 0) {
                ErrorMessage = $"La etiqueta '{invalidTags[0]}' excede los 16 caracteres.";
                return;
            }


            if (EditableTask.AppTaskTags.Count() > 3) {
                ErrorMessage = $"Has introducido más de 3 etiquetas.";
                return;
            }

            if (_isCreate) {
                EditableTask.AppContainerID = _appContainer.AppContainerID;
                EditableTask.AppTaskCreateDate = DateTime.Now;
                EditableTask.AppTaskStatus = _status;
                await SaveAddContainerAsync();
                return;
            }

            // Guardar los cambios
            await SaveEditChangesAsync();
        }
    }
}
