using MongoDB.Bson;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    class AppContainerCreateOrEditViewModel : AppContainerBaseViewModel {

        //Atributos
        private readonly bool _isCreate; //Comparar si estamo sen creación o edición

        //Nombre del espacio de trabajo.
        private string? _name;
        public string? Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public CommandViewModel SaveContainerCommand { get; } //Guardar los cambios

        //Constructor
        public AppContainerCreateOrEditViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer? appContainer) : base(databaseService, navigationService, appUser, appContainer) {
            _isCreate = appContainer == null;
            SaveContainerCommand = new CommandViewModel(async (obj) => await SaveContainerAsyncWrapper());
            Name = AppContainerEditable.AppContainerTitle;
        }

        //
        protected override async Task SaveContainerAsyncWrapper() {

            //Comprueba que el campo del nombre no este vacío
            bool fieldNameEmpty = CheckFieldName();
            if (fieldNameEmpty) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckTitleStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprueba que el campo del nombre no este en uso
            bool nameExists = await CheckNameDBB(_appUser.AppUserID);
            if (Name != AppContainerEditable.AppContainerTitle && nameExists) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckTitleContainerDBStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Si estamos en "modo creación" asigna los atributo de forma predefinida.
            AppContainerEditable.AppContainerTitle = Name;
            if (_isCreate) {
                AppContainerEditable.AppUserID = _appUser.AppUserID;
                AppContainerEditable.AppContainerAppTasksList = [];
                AppContainerEditable.AppContainerCreateDate = DateTime.Now;
                await SaveAddContainerAsync();
                return;
            }

            //Guardar los cambios
            await SaveEditChangesAsync();
        }

        //Comprobar que el título no este vacio
        private bool CheckFieldName() {
            return string.IsNullOrEmpty(Name);
        }

        //Revisar que no existe ningún título igual en los contenedores de ese usuario.
        private async Task<bool> CheckNameDBB(ObjectId appUserID) {
            return await _databaseService.CheckContainerByTitleAndUserIDAsync(Name, appUserID);
        }

    }
}
