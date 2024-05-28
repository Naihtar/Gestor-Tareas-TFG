using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public class AppTaskCreateOrEditViewModel : AppTaskBaseViewModel {

        //Atributos
        private readonly bool _isCreate; //Comparar si estamo sen creación o edición
        private readonly string? _status; //Estado obtenido de la vista

        //Comandos
        public CommandViewModel SaveTaskCommand { get; } //Guardar los cambios

        //Constructor
        public AppTaskCreateOrEditViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask? appTask, string? status)
            : base(databaseService, navigationService, appUser, appContainer, appTask) {

            _status = status;
            _isCreate = _appTask == null;

            SaveTaskCommand = new CommandViewModel(async (obj) => await SaveTaskAsyncWrapper());

        }

        protected override async Task SaveTaskAsyncWrapper() {
            // Verificar que el campo de nombre no esté vacío
            bool fieldName = CheckFieldName();

            if (fieldName) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckTitleStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            // Verificar si alguna etiqueta excede los 16 caracteres
            var invalidTags = AppTaskEditable.AppTaskTags.Where(tag => tag.Length > 16).ToList();
            if (invalidTags.Count != 0) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = (ResourceDictionary["CheckLenghtTagsStr"] as string) + $" '{invalidTags[0]}'";
                StartTimer();
                return;
            }


            if (AppTaskEditable.AppTaskTags.Count() > 3) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = (ResourceDictionary["MaxCapTagsStr"] as string);
                StartTimer();
                return;
            }

            //Si estamos en "modo creación" asigna los atributo de forma predefinida.
            if (_isCreate) {
                AppTaskEditable.AppContainerID = _appContainer.AppContainerID;
                AppTaskEditable.AppTaskCreateDate = DateTime.Now;
                AppTaskEditable.AppTaskStatus = _status;
                await SaveAddContainerAsync();
                return;
            }

            // Guardar los cambios
            await SaveEditChangesAsync();
        }

        private bool CheckFieldName() {
            return string.IsNullOrEmpty(AppTaskEditable.AppTaskTitle);
        }
    }
}
