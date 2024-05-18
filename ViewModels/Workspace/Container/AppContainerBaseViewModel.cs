using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    public abstract class AppContainerBaseViewModel : BaseViewModel {
        public CommandViewModel WorkspaceCommand { get; }

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

            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            EditableContainer = _appContainer ?? new AppContainer() {
                NombreContenedor = string.Empty,
                DescripcionContenedor = string.Empty,
                UsuarioID = _user.IdUsuario,
                ListaTareas = [],
                FechaCreacionContenedor = DateTime.Now
            };

            _navigationService = navigationService;
            _databaseService = db;
            _authenticationService = auth;
            if (container != null) {
                AppContainerData();
            }
        }

        protected async void AppContainerData() {
            AppContainer container = await _databaseService.GetContainerByIdAsync(_appContainer.IdContenedor);

            ContainerProperties = new Dictionary<string, string> {
                {"ContainerName", container.NombreContenedor },
                {"Descripcion", container.DescripcionContenedor ??= string.Empty},
                {"Fecha", container.FechaCreacionContenedor.ToString() },
            };
        }

        private void WorkspaceBack(object obj) {
            _navigationService.GoBack();
        }

        protected async Task SaveEditChangesAsync() {
            // Actualiza el usuario en la base de datos
            await _databaseService.UpdateContainerAsync(EditableContainer);

            // Actualiza las propiedades del perfil de usuario
            AppContainerData();

            //// Navega hacia atrás
            _navigationService.NavigateTo("Workspace", EditableContainer, _user, _navigationService, _databaseService, _authenticationService);
            //_navigationService.GoBack();
        }

        protected async Task SaveAddContainerAsync() {

            await _databaseService.AddContainer(EditableContainer, _user.IdUsuario);
            _navigationService.NavigateTo("Workspace", EditableContainer, _user, _navigationService, _databaseService, _authenticationService);
        }
        protected abstract Task SaveContainerAsyncWrapper();

    }
}
