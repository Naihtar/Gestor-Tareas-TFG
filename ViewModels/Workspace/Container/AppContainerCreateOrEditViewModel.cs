using MongoDB.Bson;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    class AppContainerCreateOrEditViewModel : AppContainerBaseViewModel {

        public CommandViewModel SaveCommandContainer { get; }
        private readonly bool _isCreate;

        public AppContainerCreateOrEditViewModel(AppContainer? container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) : base(container, user, navigationService, db, auth) {
            _isCreate = container == null;
            SaveCommandContainer = new CommandViewModel(async (obj) => await SaveContainerAsyncWrapper());
        }

        private bool CheckFieldName() {
            return string.IsNullOrEmpty(EditableContainer.NombreContenedor);
        }

        private async Task<bool> CheckNameDBB(ObjectId id) {
            return await _databaseService.ExistContainerWithName(EditableContainer.NombreContenedor, id);
        }

        protected override async Task SaveContainerAsyncWrapper() {

            bool name = await CheckNameDBB(_user.IdUsuario);
            bool fieldName = CheckFieldName();

            if (fieldName) {
                ErrorMessage = "Rellene el campo del nombre.";
                return;
            }

            if (name) {
                ErrorMessage = "El nombre introducido ya esta en uso.";
                return;
            }

            if (_isCreate) {
                EditableContainer.UsuarioID = _user.IdUsuario;
                EditableContainer.ListaTareas = [];
                EditableContainer.FechaCreacionContenedor = DateTime.Now;
                await SaveAddContainerAsync();
                return;
            }
            await SaveEditChangesAsync();
        }
    }
}
