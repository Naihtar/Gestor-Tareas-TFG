using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {

        //Objetos
        private readonly AppUser _appUser; //Usuario
        private AppContainer? _appContainer; //Contenedor actual

        //Dependencias
        private readonly INavigationService _navigationService; //Dependencia de los servicios navegación
        private readonly IDatabaseService _databaseService; //Dependencia de los servicios de la base de datos

        //Listas de datos
        private List<AppContainer> _userContainers; // Contenedores del usuario
        public List<string> ContainerNames {
            get {
                return _userContainers.Select(container => container.AppContainerTitle).ToList();
            }
        }

        //Comandos
        public CommandViewModel UserProfileCommand { get; private set; } //Acceder al profile
        public CommandViewModel ContainerCommand { get; private set; } //Acceder al contenedor
        public CommandViewModel ContentContainerCommand { get; private set; } //Mostrar el contenido del contenedor.
        public CommandViewModel EditContainerCommand { get; private set; } //Editar el contenido del contenedor.
        public CommandViewModel DeleteContainerCommand { get; private set; } // Eliminar el contenido del contenedor.
        public CommandViewModel AddContainerCommand { get; private set; } // Añadir el contenido del contenedor.
        public CommandViewModel ContentTaskCommand { get; private set; } //Mostrar el contenido del contenedor.
        public CommandViewModel MoveTaskCommand { get; private set; } //Mostrar el contenido del contenedor.
        public CommandViewModel AddTaskCommand { get; private set; } //Añadir el contenido de una task.

        //Colecciones visuales en la vista.
        public ObservableCollection<AppTask> ToDoTasks { get; private set; } //TODO List
        public ObservableCollection<AppTask> InProgressTasks { get; private set; } //In Progress List 
        public ObservableCollection<AppTask> DoneTasks { get; private set; } //DONE List
        public ObservableCollection<AppTask> OnHoldTasks { get; private set; } //On Hold List

        //Atributos: View
        private string? _selectedContainerName; // Nombre del contenedor.
        public string SelectedContainerName {
            get {
                return $"{ResourceDictionary["WorkspaceStr"]}: {_selectedContainerName}";
            }
            set {
                if (_selectedContainerName != value) {
                    _selectedContainerName = value;
                    OnPropertyChanged(nameof(SelectedContainerName)); // Notifica que la propiedad ha cambiado
                }
            }
        }


        private AppTask? _selectedTask;
        public AppTask? SelectedTask {
            get { return _selectedTask; }
            set {
                if (_selectedTask != value) {
                    _selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                }
            }
        }

        private bool _hasContainer;
        public bool HasContainer {
            get { return _hasContainer; }
            set {
                if (_hasContainer != value) {
                    _hasContainer = value;
                    OnPropertyChanged(nameof(HasContainer));
                }
            }
        }
        private bool _maxContainer;
        public bool MaxContainer {
            get { return _maxContainer; }
            set {
                if (_maxContainer != value) {
                    _maxContainer = value;
                    OnPropertyChanged(nameof(MaxContainer));
                }
            }
        }

        // Constructor principal.
        private WorkSpaceViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, string? successMessage) {
            //Objetos
            _appUser = appUser;
            _appContainer = null; //Es null, para controlar ContainerAccess
            _selectedTask = null;
            //Servicios
            _navigationService = navigationService; //Servicio encargado de la navegación entre ventanas.
            _databaseService = databaseService; // Servicio encargado de la obtención de información desde la BDD.

            //Comandos
            UserProfileCommand = new CommandViewModel(UserProfileAccess); // Acceder al perfil de usuario
            ContentContainerCommand = new CommandViewModel(ContentContainers); // Mostrar el contenido del contenedor
            ContainerCommand = new CommandViewModel(ContainerAccess); //Acceder a la información del contenedor
            EditContainerCommand = new CommandViewModel(ContainerEdit); //Acceder a la información del contenedor
            DeleteContainerCommand = new CommandViewModel(async (obj) => await DeleteContainer());
            AddContainerCommand = new CommandViewModel(AddContainer);
            ContentTaskCommand = new CommandViewModel(TaskAccess); // Mostrar el contenido del contenedor
            MoveTaskCommand = new CommandViewModel(MoveTaskToNextList);
            AddTaskCommand = new CommandViewModel(CreateTask);

            //Atributos: View
            SelectedContainerName = string.Empty; // Nombre del contenedor

            //Lista de datos
            _userContainers = []; //Lista de contenedores que tiene el usuario

            //Colecciones visuales en la vista. (ObservationCollection).
            ToDoTasks = [];
            InProgressTasks = [];
            DoneTasks = [];
            OnHoldTasks = [];

            Message = successMessage;
            if (Message != null) {
                ShowSuccessMessage(Message);
            }
        }

        // Constructor adicional para cargar tareas cuando se regresa a la página del espacio de trabajo
        public WorkSpaceViewModel(IDatabaseService db, INavigationService nav, AppUser user, AppContainer container, string? successMessage) : this(db, nav, user, successMessage) {
            _appContainer = container;
            SelectedContainerName = container.AppContainerTitle; // Actualizar el nombre del contenedor seleccionado

            // Cargar las tareas del contenedor seleccionado
            LoadTasksForContainer();
        }

        //Método para inicializar el workspace de forma asincrona
        public static async Task<WorkSpaceViewModel> CreateAsync(IDatabaseService db, INavigationService nav, AppUser user, AppContainer container, string? successMessage) {
            WorkSpaceViewModel viewModel = container != null
                ? new WorkSpaceViewModel(db, nav, user, container, successMessage) // Si hay contenedor, crea una instancia del ViewModel con el contenedor.
                : new WorkSpaceViewModel(db, nav, user, successMessage); // Si no hay contenedor, crea una instancia del ViewModel sin contenedor.

            await viewModel.InitializeAsync(); //Ejecutamos el método para obtener la información de los contenedores y otros atributos.
            return viewModel; //Devolvemos el viewModel.
        }

        //Obtener contenedores.
        private async Task InitializeAsync() {
            try {
                _userContainers = await ObtainContainerDB().ConfigureAwait(false);
                OnPropertyChanged(nameof(ContainerNames));
                HasContainers(_userContainers);
                MaxContainers(_userContainers);
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = (ResourceDictionary["ExDB"] as string);
                StartTimer();
            }
        }

        private async Task<List<AppContainer>> ObtainContainerDB() {
            try {
                List<ObjectId> containerIds = await _databaseService.GetContainerListByUserIDAsync(_appUser.AppUserID).ConfigureAwait(false);
                List<AppContainer> containers = [];
                if (containerIds != null && containerIds.Count > 0) {
                    foreach (var id in containerIds) {
                        var container = await _databaseService.GetContainerByContainerIDAsync(id);
                        if (container != null) {
                            containers.Add(container);
                        }
                    }
                }
                HasContainers(containers);
                MaxContainers(containers);
                return containers;
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = (ResourceDictionary["ExDB"] as string);
                StartTimer();
                return [];
            }
        }



        // Método para cargar las tareas del contenedor seleccionado
        private async void LoadTasksForContainer() {
            try {
                if (_appContainer != null) {

                    //Comprbar que el contenedor tiene tareas
                    bool hasTask = await _databaseService.VerifyTaskInContainerAsync(_appContainer.AppContainerID);
                    // Limpiar colecciones
                    ClearTasks();
                    List<AppTask> tasks = [];

                    if (hasTask) {
                        // Obtener desde la base de datos las tareas del contenedor seleccionado
                        tasks = await _databaseService.GetTasksByContainerIdAsync(_appContainer.AppContainerID);
                        // Iterarlas y asignarlas a la colección correspondiente.
                        foreach (var task in tasks) {
                            switch (task.AppTaskStatus) {
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
                                    SuccessOpen = false;
                                    ErrorOpen = true;
                                    ErrorMessage = (ResourceDictionary["LoadWorkspaceTaskStr"] as string) + $": {task.AppTaskTitle}";
                                    StartTimer();
                                    break;
                            }
                        }
                    }
                }
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = (ResourceDictionary["ExDB"] as string);
                StartTimer();
            }

        }

        //Asignar el contenido a los contenedores y acceder al contenedor deseado.
        private void ContentContainers(object parameter) {
            string containerName = (string)parameter;
            if (containerName != null) {
                //Seleccionar el contenedor que vamos a visualizar.
                var selectedContainer = _userContainers.FirstOrDefault(container => container.AppContainerTitle == containerName);
                _appContainer = selectedContainer; //Asignar ese contenedor.
                if (selectedContainer != null) {
                    SelectedContainerName = selectedContainer.AppContainerTitle; //Obtener el nombre.

                    // Cargar las tareas del contenedor seleccionado
                    LoadTasksForContainer();
                }
            }
        }


        //Método para acceder al perfil de usuario
        private void UserProfileAccess(object obj) {
            _navigationService.NavigateTo("Profile", appUser: _appUser, appContainer: _appContainer, appTask: SelectedTask, null); //Ir a la vista del perfil
        }

        //Método para acceder a la información del contenedor
        private void ContainerAccess(object obj) {

            // Indicar al usuario que no ha seleccionado nada.
            if (_appContainer == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["NoSelectWorkspaceContainerStr"] as string;
                StartTimer();
                return;
            }
            _navigationService.NavigateTo("Container", _appUser, _appContainer); //Ir a la vista del container
        }

        private void ContainerEdit(object obj) {

            if (_appContainer == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["NoSelectWorkspaceContainerStr"] as string;
                StartTimer();
                return;
            }
            _navigationService.NavigateTo("ContainerEdit", appUser: _appUser, appContainer: _appContainer); //Ir a la vista del container
        }

        //Delete container
        private async Task DeleteContainer() {

            if (_appContainer == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["NoSelectWorkspaceContainerStr"] as string;
                StartTimer();
                return;
            }

            // Llama al método correspondiente en IDatabaseService para realizar el borrado en cascada
            bool success = await _databaseService.DeleteContainerRecursiveAsync(_appContainer.AppContainerID, _appUser.AppUserID);

            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string;
                StartTimer();
                return;
            }
            // Elimina el contenedor de la lista de contenedores del usuario
            _userContainers.Remove(_appContainer);

            // Limpia las colecciones de tareas
            ClearTasks();

            // Actualiza la interfaz de usuario
            OnPropertyChanged(nameof(ContainerNames));
            SelectedContainerName = string.Empty;
            MaxContainers(_userContainers);

            // Muestra un mensaje de confirmación
            string? msg = ResourceDictionary["SuccessDeleteContainerInfoBarStr"] as string;
            _appContainer?.Dispose();
            _navigationService.NavigateTo(appUser: _appUser, appContainer: null, successMessage: msg);

        }

        private void AddContainer(object obj) {
            if (MaxContainer) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["MaxCapWorkspaceContainerStr"] as string;
                StartTimer();
                return;
            }
            _navigationService.NavigateTo("ContainerAdd", appUser: _appUser, appContainer: null); //Ir a la vista del container
        }

        //Tasks

        private void TaskAccess(object obj) {
            if (obj is not AppTask task) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["WorkspaceTaskStr"] as string;
                StartTimer();
                return;
            }
            SelectedTask = task;
            _navigationService.NavigateTo("Task", appUser: _appUser, appContainer: _appContainer, appTask: SelectedTask, null); //Ir a la vista del container
            SelectedTask = null;
        }
        private void CreateTask(object obj) {
            HasContainer = _userContainers.Count > 0;

            if (_appContainer == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["NoSelectWorkspaceContainerStr"] as string;
                StartTimer();
                return;
            }
            string? status = obj.ToString(); // TODO Editar
            _navigationService.NavigateTo("TaskAdd", _appUser, _appContainer, null, status); //Ir a la vista del container
            SelectedTask = null;
            LoadTasksForContainer();
        }

        // Método que realiza el movimiento de la tarea
        private async void MoveTaskToNextList(object obj) {
            if (_selectedTask == null) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["NoSelectWorkspaceTaskStr"] as string;
                StartTimer();
                return;
            }

            string? status = obj.ToString();

            _selectedTask.AppTaskStatus = status;

            // Update the task in the database
            bool isUpdated = await _databaseService.UpdateTaskAsync(_selectedTask);

            if (!isUpdated) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string;
                StartTimer();
                return;
            }
            LoadTasksForContainer();
        }

        private void ClearTasks() {
            ToDoTasks.Clear();
            InProgressTasks.Clear();
            OnHoldTasks.Clear();
            DoneTasks.Clear();
        }

        private void HasContainers(List<AppContainer> containers) {
            HasContainer = containers.Count > 0;
        }
        private void MaxContainers(List<AppContainer> containers) {
            MaxContainer = containers.Count >= 5;
        }
    }
}

