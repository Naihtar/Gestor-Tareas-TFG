using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    public abstract class AppContainerBaseViewModel : BaseViewModel {
        public CommandViewModel GoBackCommand { get; }

        protected AppContainer? _appContainer;
        protected AppUser _user;
        protected readonly IDatabaseService _databaseService;
        protected readonly INavigationService _navigationService;
        protected readonly IAuthenticationService _authenticationService;

        public AppContainer EditableContainer { get; set; }

        private Dictionary<string, string>? _containerProperties;
        public Dictionary<string, string>? ContainerProperties {
            get { return _containerProperties; }
            set {
                _containerProperties = value;
                OnPropertyChanged(nameof(ContainerProperties));
            }
        }
        protected AppContainerBaseViewModel(AppContainer? container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) {
            _appContainer = container;
            _user = user;

            GoBackCommand = new CommandViewModel(GoBack);
            EditableContainer = _appContainer ?? new AppContainer() {
                AppContainerTitle = string.Empty,
                AppContainerDescription = string.Empty,
                AppUserID = _user.AppUserID,
                AppContainerAppTasksList = [],
                AppContainerCreateDate = DateTime.Now
            };

            _navigationService = navigationService;
            _databaseService = db;
            _authenticationService = auth;
            if (container != null) {
                AppContainerData();
            }
        }

        protected async void AppContainerData() {
            AppContainer? container = await _databaseService.GetContainerByContainerIDAsync(_appContainer.AppContainerID);

            if (container == null) {
                ErrorMessage = "Ha ocurrido un error al obtener el espacio de traabajo";
                return;
            }

            ContainerProperties = new Dictionary<string, string> {
                {"ContainerName", container.AppContainerTitle },
                {"Descripcion", container.AppContainerDescription ??= string.Empty},
                {"Fecha", container.AppContainerCreateDate.ToString() },
            };
        }

        private void GoBack(object obj) {
            _navigationService.GoBack();
        }

        protected async Task SaveEditChangesAsync() {
            // Actualiza el usuario en la base de datos
            bool success = await _databaseService.UpdateContainerAsync(EditableContainer);

            if (!success) {
                ErrorMessage = "Ha ocurrido un error al guardar la edición del Workspace.";
                return;
            }

            // Actualiza las propiedades del perfil de usuario
            AppContainerData();

            //// Navega hacia atrás
            _navigationService.NavigateTo("Workspace", EditableContainer, _user, _navigationService, _databaseService, _authenticationService);
            //_navigationService.GoBack();
        }

        protected async Task SaveAddContainerAsync() {

            bool success = await _databaseService.AddContainerAsync(EditableContainer, _user.AppUserID);

            if (!success) {
                ErrorMessage = "Ha ocurrido un error al crear el nuevo Workspace.";
                return;
            }

            _navigationService.NavigateTo("Workspace", EditableContainer, _user, _navigationService, _databaseService, _authenticationService);
        }
        protected abstract Task SaveContainerAsyncWrapper();

    }
}
