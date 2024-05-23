using MongoDB.Bson;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    class AppContainerCreateOrEditViewModel : AppContainerBaseViewModel {

        public CommandViewModel SaveContainerCommand { get; }
        private readonly bool _isCreate;

        private string? _name;
        public string? Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AppContainerCreateOrEditViewModel(AppContainer? container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) : base(container, user, navigationService, db, auth) {
            _isCreate = container == null;
            SaveContainerCommand = new CommandViewModel(async (obj) => await SaveContainerAsyncWrapper());
            Name = EditableContainer.AppContainerTitle;
        }

        private bool CheckFieldName() {
            return string.IsNullOrEmpty(Name);
        }

        private async Task<bool> CheckNameDBB(ObjectId id) {
            return await _databaseService.CheckContainerByTitleAndUserIDAsync(Name, id);
        }

        protected override async Task SaveContainerAsyncWrapper() {
            bool nameExists = await CheckNameDBB(_user.AppUserID);
            bool fieldNameEmpty = CheckFieldName();

            if (fieldNameEmpty) {
                ErrorMessage = "Rellene el campo del nombre.";
                return;
            }

            if (Name != EditableContainer.AppContainerTitle && nameExists) {
                ErrorMessage = "El nombre introducido ya esta en uso.";
                return;
            }

            EditableContainer.AppContainerTitle = Name;
            if (_isCreate) {
                EditableContainer.AppUserID = _user.AppUserID;
                EditableContainer.AppContainerAppTasksList = [];
                EditableContainer.AppContainerCreateDate = DateTime.Now;
                await SaveAddContainerAsync();
            } else {
                await SaveEditChangesAsync();
            }
        }

    }
}
