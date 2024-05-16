using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Windows;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {

        //Objetos
        private readonly AppUser _user; //Usuario
        private AppContainer? _appContainer; //Contenedor actual

        //Servicioes
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService _authenticationService;

        //Listas de datos
        private List<AppContainer> _userContainers; // Contenedores del usuario
        private List<string> _containerNames; // Nombre de los contenedores
        public List<string> ContainerNames {
            get {
                _containerNames.Clear();
                foreach (var container in _userContainers) {
                    _containerNames.Add(container.NombreContenedor);
                }
                return _containerNames;
            }
        }

        //Comandos
        public CommandViewModel UserProfileCommand { get; private set; } //Acceder al profile
        public CommandViewModel ContainerCommand { get; private set; } //Acceder al contenedor
        public CommandViewModel ShowContentContainer { get; private set; } //Mostrar el contenido del contenedor.
        public CommandViewModel EditContainer { get; private set; } //Editar el contenido del contenedor.
        public CommandViewModel DeleteContainerCommand { get; private set; } // Eliminar el contenido del contenedor.
        public CommandViewModel AddContainerCommand { get; private set; } // Añadir el contenido del contenedor.

        //Colecciones visuales en la vista.
        public ObservableCollection<AppTask> ToDoTasks { get; private set; } //TODO List
        public ObservableCollection<AppTask> InProgressTasks { get; private set; } //In Progress List 
        public ObservableCollection<AppTask> DoneTasks { get; private set; } //DONE List
        public ObservableCollection<AppTask> OnHoldTasks { get; private set; } //On Hold List

        //Atributos: View
        private string? _selectedContainerName; // Nombre del contenedor.
        public string SelectedContainerName {
            get { return ($"Workspace: {_selectedContainerName}"); }
            set {
                if (_selectedContainerName != value) {
                    _selectedContainerName = value;
                    OnPropertyChanged(nameof(SelectedContainerName)); // Notifica que la propiedad ha cambiado
                }
            }
        }

        // Constructor principal.
        private WorkSpaceViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) {

            //Objetos
            _user = user;
            _appContainer = null; //Es null, para controlar ContainerAccess

            //Servicios
            _navigationService = nav; //Servicio encargado de la navegación entre ventanas.
            _databaseService = db; // Servicio encargado de la obtención de información desde la BDD.
            _authenticationService = auth; //Servicio de authentificación.
            //Comandos
            UserProfileCommand = new CommandViewModel(UserProfileAccess); // Acceder al perfil de usuario
            ShowContentContainer = new CommandViewModel(ContentContainers); // Mostrar el contenido del contenedor
            ContainerCommand = new CommandViewModel(ContainerAccess); //Acceder a la información del contenedor
            EditContainer = new CommandViewModel(ContainerEdit); //Acceder a la información del contenedor
            DeleteContainerCommand = new CommandViewModel(async (obj) => await DeleteContainer());
            AddContainerCommand = new CommandViewModel(AddContainer);

            //Atributos: View
            SelectedContainerName = string.Empty; // Nombre del contenedor

            //Lista de datos
            _userContainers = []; //Lista de contenedores que tiene el usuario
            _containerNames = []; //Lista de los nombres de los contenedores

            //Colecciones visuales en la vista. (ObservationCollection).
            ToDoTasks = [];
            InProgressTasks = [];
            DoneTasks = [];
            OnHoldTasks = [];
        }

        // Constructor adicional para cargar tareas cuando se regresa a la página del espacio de trabajo
        public WorkSpaceViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppContainer container) : this(user, nav, db, auth) {
            _appContainer = container;
            SelectedContainerName = container.NombreContenedor; // Actualizar el nombre del contenedor seleccionado

            // Cargar las tareas del contenedor seleccionado
            LoadTasksForContainer();

        }

        //Método para inicializar el workspace de forma asincrona
        public static async Task<WorkSpaceViewModel> CreateAsync(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppContainer container) {
            WorkSpaceViewModel viewModel = container != null
                ? new WorkSpaceViewModel(user, nav, db, auth, container) // Si hay contenedor, crea una instancia del ViewModel con el contenedor.
                : new WorkSpaceViewModel(user, nav, db, auth); // Si no hay contenedor, crea una instancia del ViewModel sin contenedor.

            await viewModel.InitializeAsync(); //Ejecutamos el método para obtener la información de los contenedores y otros atributos.
            return viewModel; //Devolvemos el viewModel.
        }

        //Obtener contenedores.
        private async Task InitializeAsync() {
            _userContainers = await ObtainContainerDB(); //Asignamos la lista de contenedores.
            _containerNames = ContainerNames; //Obtenemos los nombres de los contenedores.
        }

        //Obtener desde la base de datos los contenedores vía ID del listado de contenedores del usuario.
        private async Task<List<AppContainer>> ObtainContainerDB() {
            List<ObjectId> containerIds = await _databaseService.GetListContianerUser(_user.IdUsuario); // Listado de IDs de los contenedores del usuario
            List<AppContainer> containers = []; //Lista de contenedores.
            if (containerIds != null && containerIds.Count > 0) {
                foreach (var id in containerIds) {
                    var container = await _databaseService.GetContainerByIdAsync(id); //Obtener el contenedor por el ID.
                    if (container != null) {
                        containers.Add(container);
                    }
                }
            }
            return containers; // Devuelve la lista de contendores.
        }


        // Método para cargar las tareas del contenedor seleccionado
        private async void LoadTasksForContainer() {
            if (_appContainer != null) {
                // Limpiar colecciones
                ToDoTasks.Clear();
                InProgressTasks.Clear();
                OnHoldTasks.Clear();
                DoneTasks.Clear();

                if (_appContainer.ListaTareas.Count > 0) {
                    // Obtener desde la base de datos las tareas del contenedor seleccionado
                    List<AppTask> tasks = await _databaseService.GetTasksByContainerIdAsync(_appContainer.IdContenedor);

                    // Iterarlas y asignarlas a la colección correspondiente.
                    foreach (var task in tasks) {
                        switch (task.EstadoTarea) {
                            case "Pendiente":
                                ToDoTasks.Add(task);
                                break;
                            case "En progreso":
                                InProgressTasks.Add(task);
                                break;
                            case "En espera":
                                OnHoldTasks.Add(task);
                                break;
                            case "Completado":
                                DoneTasks.Add(task);
                                break;
                            default:
                                MessageBox.Show($"Hay un error con la tarea: {task.NombreTarea}");
                                break;
                        }
                    }
                }
            }
        }

        //Asignar el contenido a los contenedores y acceder al contenedor deseado.
        private void ContentContainers(object parameter) {
            string containerName = (string)parameter;
            if (containerName != null) {
                //Seleccionar el contenedor que vamos a visualizar.
                var selectedContainer = _userContainers.FirstOrDefault(container => container.NombreContenedor == containerName);
                _appContainer = selectedContainer; //Asignar ese contenedor.
                if (selectedContainer != null) {
                    SelectedContainerName = selectedContainer.NombreContenedor; //Obtener el nombre.

                    // Cargar las tareas del contenedor seleccionado
                    LoadTasksForContainer();
                }
            }
        }

        //Método para acceder al perfil de usuario
        private void UserProfileAccess(object obj) {
            _navigationService.NavigateTo("Profile", _user, _navigationService, _databaseService, _authenticationService); //Ir a la vista del perfil
        }

        //Método para acceder a la información del contenedor
        private void ContainerAccess(object obj) {

            // Indicar al usuario que no ha seleccionado nada.
            if (_appContainer == null) {
                MessageBox.Show($"No has seleccionado ningún contenedor");
                return;
            }
            _navigationService.NavigateTo("Container", _appContainer, _user, _navigationService, _databaseService, _authenticationService); //Ir a la vista del container
        }

        private void ContainerEdit(object obj) {

            if (_appContainer == null) {
                MessageBox.Show($"No has seleccionado ningún contenedor");
                return;
            }
            _navigationService.NavigateTo("ContainerEdit", _appContainer, _user, _navigationService, _databaseService, _authenticationService); //Ir a la vista del container
        }

        //Delete container
        private async Task DeleteContainer() {
            if (_appContainer != null) {
                // Llama al método correspondiente en IDatabaseService para realizar el borrado en cascada
                bool success = await _databaseService.DeleteContainerAndTasks(_appContainer.IdContenedor, _user.IdUsuario);

                if (success) {
                    // Elimina el contenedor de la lista de contenedores del usuario
                    _userContainers.Remove(_appContainer);

                    // Limpia las colecciones de tareas
                    ToDoTasks.Clear();
                    InProgressTasks.Clear();
                    OnHoldTasks.Clear();
                    DoneTasks.Clear();

                    // Actualiza la interfaz de usuario
                    OnPropertyChanged(nameof(ContainerNames));
                    SelectedContainerName = string.Empty;

                    // Muestra un mensaje de confirmación
                    MessageBox.Show("El contenedor y sus elementos asociados se han eliminado correctamente.");
                    _navigationService.NavigateTo("Workspace", _user, _navigationService, _databaseService, _authenticationService);
                }
            }
        }

        private void AddContainer(object obj) {
            _navigationService.NavigateTo("ContainerAdd", null, _user, _navigationService, _databaseService, _authenticationService); //Ir a la vista del container
        }
    }
}
