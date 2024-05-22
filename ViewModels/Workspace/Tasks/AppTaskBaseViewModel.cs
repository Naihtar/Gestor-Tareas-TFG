using System.Reflection.Metadata;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public abstract class AppTaskBaseViewModel : BaseViewModel {
        public CommandViewModel GoBackCommand { get; }
        public CommandViewModel SetStateCommand { get; }
        protected AppContainer _appContainer;
        protected AppTask? _appTask;
        protected AppUser _user;
        protected readonly IDatabaseService _databaseService;
        protected readonly INavigationService _navigationService;
        protected readonly IAuthenticationService _authenticationService;
        protected string _statusTask = "Pendiente";
        public AppTask EditableTask { get; set; }
        private string _tagsTask;

        public string TagsTask {
            get => _tagsTask;
            set {
                if (_tagsTask != value) {
                    _tagsTask = value;
                    OnPropertyChanged(nameof(TagsTask));
                    UpdateTagsFromText();
                }
            }
        }

        private Dictionary<string, string>? _taskProperties;
        public Dictionary<string, string>? TaskProperties {
            get { return _taskProperties; }
            set {
                _taskProperties = value;
                OnPropertyChanged(nameof(TaskProperties));
            }
        }

        protected AppTaskBaseViewModel(AppTask? task, AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) {
            _appContainer = container;
            _appTask = task;
            _user = user;
            GoBackCommand = new CommandViewModel(GoBack);
            //SetStateCommand = new CommandViewModel(OnButtonPressed);
            EditableTask = _appTask ?? new AppTask() {
                NombreTarea = string.Empty,
                DescripcionTarea = string.Empty,
                FechaCreacionTarea = DateTime.Now,
                EstadoTarea = _statusTask,
                ContenedorID = _appContainer.IdContenedor,
                EtiquetasTarea = Array.Empty<string>(), // Initialize as empty array
            };

            _tagsTask = string.Empty;
            _navigationService = navigationService;
            _databaseService = db;
            _authenticationService = auth;

            if (_appTask != null) {
                AppTaskData();
            }

            // Initialize TagsTask from EditableTask
            TagsTask = string.Join(", ", EditableTask.EtiquetasTarea);
        }

        protected async void AppTaskData() {
            AppTask task = await _databaseService.GetTastkByIdAsync(_appTask.IdTarea);
            TaskProperties = new Dictionary<string, string>
            {
            { "Nombre:", task.NombreTarea },
            { "Descripcion:", task.DescripcionTarea },
            { "Fecha:", task.FechaCreacionTarea.ToString() },
            { "Tags:", string.Join(", ", task.EtiquetasTarea.Select(etiqueta => $"#{etiqueta}")) }
        };
        }

        protected async Task SaveEditChangesAsync() {

            bool success = await _databaseService.UpdateTaskAsync(EditableTask);

            if (!success) {
                ErrorMessage = "Ha ocurrido un error inesperado";
                return;
            }
            _navigationService.NavigateTo("Workspace", _appContainer, _user, _navigationService, _databaseService, _authenticationService);
        }

        protected async Task SaveAddContainerAsync() {
            await _databaseService.AddTask(EditableTask, _appContainer.IdContenedor);
            _navigationService.NavigateTo("Workspace", _appContainer, _user, _navigationService, _databaseService, _authenticationService);
        }

        //private void OnButtonPressed(object obj) {
        //    string? buttonNumber = obj.ToString();
        //    _statusTask = buttonNumber;
        //}

        protected abstract Task SaveTaskAsyncWrapper();

        private void GoBack(object obj) {
            _navigationService.GoBack();
        }

        protected void UpdateTagsFromText() {
            var tags = TagsTask.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(tag => tag.Trim().Replace("#", "").Replace(" ", "_"))
                               .ToArray();

            EditableTask.EtiquetasTarea = tags;
            TagsTask = string.Join(", ", tags);
        }
    }

}
