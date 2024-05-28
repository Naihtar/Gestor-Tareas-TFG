using System.Reflection.Metadata;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public abstract class AppTaskBaseViewModel : BaseViewModel {

        //Dependencias
        protected readonly IDatabaseService _databaseService; //Dependencia de los servicios de la gestión de la base de datos
        protected readonly INavigationService _navigationService; //Dependencia de los servicios navegación

        //Atributos
        protected AppUser _appUser;
        protected AppContainer _appContainer;
        protected AppTask? _appTask;
        protected string _statusTask = "Pendiente";

        private string _tagsTask; //Lista de etiquetas
        public string TagsTask {
            get => _tagsTask;
            set {
                if (_tagsTask != value) {
                    _tagsTask = value;
                    OnPropertyChanged(nameof(TagsTask));
                    UpdateTagsFromText(); //Actualizar la lista de etiquetas del textbox
                }
            }
        }
        public AppTask AppTaskEditable { get; set; } //Tarea modificable

        //Diccionario para mostrar los datos del espacio de trabajo
        private Dictionary<string, string>? _taskProperties;
        public Dictionary<string, string>? TaskProperties {
            get { return _taskProperties; }
            set {
                _taskProperties = value;
                OnPropertyChanged(nameof(TaskProperties));
            }
        }

        //Comandos
        public CommandViewModel GoBackCommand { get; }

        //Constructores
        protected AppTaskBaseViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask? appTask) {
            _navigationService = navigationService;
            _databaseService = databaseService;

            _appContainer = appContainer;
            _appTask = appTask;
            _appUser = appUser;

            //Asignamos la tarea parametrizada, en caso de ser null se asigna una predefinida
            AppTaskEditable = _appTask ?? new AppTask() {
                AppTaskTitle = string.Empty,
                AppTaskDescription = string.Empty,
                AppTaskCreateDate = DateTime.Now,
                AppTaskStatus = _statusTask,
                AppContainerID = _appContainer.AppContainerID,
                AppTaskTags = [], // Initialize as empty array
            };

            _tagsTask = string.Empty;
            TagsTask = string.Join(", ", AppTaskEditable.AppTaskTags);

            GoBackCommand = new CommandViewModel(GoBack);
            if (_appTask != null) {
                AppTaskData();
            }
        }

        protected async void AppTaskData() {
            try {
                AppTask? appTaskData = await _databaseService.GetTastkByIdAsync(_appTask.AppTaskID);
                if (appTaskData == null) {
                    SuccessOpen = false;
                    ErrorOpen = true;
                    ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                    StartTimer();
                    return;
                }

                TaskProperties = new Dictionary<string, string>{
                    { "Nombre:", appTaskData.AppTaskTitle },
                    { "Descripcion:", appTaskData.AppTaskDescription },
                    { "Fecha:", appTaskData.AppTaskCreateDate.ToShortDateString() },
                    { "Tags:", string.Join(", ", appTaskData.AppTaskTags.Select(etiqueta => $"#{etiqueta}")) } //Usamos LINQ para mostrar las etiquetas con el estilo deseado.
                };
            } catch (Exception) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
            }
        }

        protected async Task SaveEditChangesAsync() {
            // Actualiza la tarea en la base de datos
            bool success = await _databaseService.UpdateTaskAsync(AppTaskEditable);
            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            //Volver a la vista de espacio de trabajo, con los datos cargados
            string? msgChanges = ResourceDictionary["SuccessTaskChangeInfoBarStr"] as string; //Mensaje de modificación exitosa
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, msgChanges);
        }

        protected async Task SaveAddContainerAsync() {
            bool success = await _databaseService.AddTaskAsync(AppTaskEditable, _appContainer.AppContainerID);
            //Agrega la tarea a la base de datos
            if (!success) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ExDB"] as string; //Mensaje de error por parte de la DB
                StartTimer();
                return;
            }

            //Volver a la vista de espacio de trabajo, con los datos cargados
            string? msgChanges = ResourceDictionary["SuccessTaskAddInfoBarStr"] as string;  //Mensaje de agregación exitosa
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, msgChanges);
        }


        protected abstract Task SaveTaskAsyncWrapper(); //Guardar los cambios en la base de datos

        private void GoBack(object obj) {
            //Volver atrás
            _navigationService.GoBack();
        }

        protected void UpdateTagsFromText() {

            //Divide el texto introducido, usando de referencia las comas, y tras hacerlo, los asigna en un array
            var tags = TagsTask.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(appTaskTags => appTaskTags.Trim().Replace("#", "").Replace(" ", "_"))
                               .ToArray();

            AppTaskEditable.AppTaskTags = tags; //Asignamos esta operación, a las etiquetas de la tarea.
            TagsTask = string.Join(", ", tags);
        }
    }

}
