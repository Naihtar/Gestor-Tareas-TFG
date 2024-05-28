using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Container {
    public abstract class AppContainerBaseViewModel : BaseViewModel {

        //Dependencias
        protected readonly IDatabaseService _databaseService; //Dependencia de los servicios de la gestión de la base de datos
        protected readonly INavigationService _navigationService; //Dependencia de los servicios navegación

        //Atributos
        protected AppContainer? _appContainer;
        protected AppUser _appUser;

        public AppContainer AppContainerEditable { get; set; } //Espacio de datos modificable

        //Diccionario para mostrar los datos del espacio de trabajo
        private Dictionary<string, string>? _containerProperties;
        public Dictionary<string, string>? ContainerProperties {
            get { return _containerProperties; }
            set {
                _containerProperties = value;
                OnPropertyChanged(nameof(ContainerProperties));
            }
        }

        //Comandos
        public CommandViewModel GoBackCommand { get; }

        //Constructor
        protected AppContainerBaseViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser user, AppContainer? appContainer) {
            _navigationService = navigationService;
            _databaseService = databaseService;
            _appContainer = appContainer;
            _appUser = user;

            //Asignamos el espacio de trabajo parametrizado, en caso de ser null se asigna uno predefinido
            AppContainerEditable = _appContainer ?? new AppContainer() {
                AppContainerTitle = string.Empty,
                AppContainerDescription = string.Empty,
                AppUserID = _appUser.AppUserID,
                AppContainerAppTasksList = [],
                AppContainerCreateDate = DateTime.Now
            };

            GoBackCommand = new CommandViewModel(GoBack);
            //Cargar los datos en caso de que no sea null
            if (appContainer != null) {
                AppContainerData();
            }
        }

        //Cargar los datos del espacio de trabajo
        protected async void AppContainerData() {
            try {
                //Buscar el espacio de trabajo en la DB por ID
                AppContainer? appContainerData = await _databaseService.GetContainerByContainerIDAsync(_appContainer.AppContainerID);

                if (appContainerData == null) {
                    SuccessOpen = false;
                    ErrorOpen = true;
                    ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                    StartTimer();
                    return;
                }

                ContainerProperties = new Dictionary<string, string> {
                {"ContainerName", appContainerData.AppContainerTitle },
                {"Descripcion", appContainerData.AppContainerDescription ??= string.Empty},
                {"Fecha", appContainerData.AppContainerCreateDate.ToShortDateString() },
            };
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
            }
        }

        protected async Task SaveEditChangesAsync() {
            // Actualiza el usuario en la base de datos
            bool success = await _databaseService.UpdateContainerAsync(AppContainerEditable);

            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            // Actualiza las propiedades del perfil de usuario
            AppContainerData();

            //Volver a la vista de espacio de trabajo, con los datos cargados
            string? msgChanges = ResourceDictionary["SuccessContainerChangeInfoBarStr"] as string; //Mensaje de modificación exitosa
            _navigationService.NavigateTo(appUser: _appUser, appContainer: AppContainerEditable, msgChanges);
        }

        protected async Task SaveAddContainerAsync() {
            //Agrega el nuevo contenedor a la base de datos
            bool success = await _databaseService.AddContainerAsync(AppContainerEditable, _appUser.AppUserID);

            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            //Volver a la vista de espacio de trabajo, con los datos cargados
            string? msgChanges = ResourceDictionary["SuccessContainerAddInfoBarStr"] as string; //Mensaje de agregación exitosa
            _navigationService.NavigateTo(appUser: _appUser, appContainer: AppContainerEditable, msgChanges);
        }
        protected abstract Task SaveContainerAsyncWrapper(); //Guardar los cambios en la base de datos
        private void GoBack(object obj) {
            //Volver atrás
            _navigationService.GoBack();
        }
    }
}
