using System.Reflection.Metadata;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public abstract class AppTaskBaseViewModel : BaseViewModel {
        public CommandViewModel GoBackCommand { get; }
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
            EditableTask = _appTask ?? new AppTask() {
                AppTaskTitle = string.Empty,
                AppTaskDescription = string.Empty,
                AppTaskCreateDate = DateTime.Now,
                AppTaskStatus = _statusTask,
                AppContainerID = _appContainer.AppContainerID,
                AppTaskTags = Array.Empty<string>(), // Initialize as empty array
            };

            _tagsTask = string.Empty;
            _navigationService = navigationService;
            _databaseService = db;
            _authenticationService = auth;

            if (_appTask != null) {
                AppTaskData();
            }

            // Initialize TagsTask from EditableTask
            TagsTask = string.Join(", ", EditableTask.AppTaskTags);
        }

        protected async void AppTaskData() {
            AppTask task = await _databaseService.GetTastkByIdAsync(_appTask.AppTaskID);
            if (task == null) {
                ErrorMessage = "Ha ocurrido un error al obtener la tarea";
                return;
            }

            TaskProperties = new Dictionary<string, string>
            {
            { "Nombre:", task.AppTaskTitle },
            { "Descripcion:", task.AppTaskDescription },
            { "Fecha:", task.AppTaskCreateDate.ToString() },
            { "Tags:", string.Join(", ", task.AppTaskTags.Select(etiqueta => $"#{etiqueta}")) }
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
          bool success =  await _databaseService.AddTaskAsync(EditableTask, _appContainer.AppContainerID);
            if (!success) {
                ErrorMessage = "Ha ocurrido un error inesperado";
                return;
            }
            _navigationService.NavigateTo("Workspace", _appContainer, _user, _navigationService, _databaseService, _authenticationService);
        }



        protected abstract Task SaveTaskAsyncWrapper();

        private void GoBack(object obj) {
            _navigationService.GoBack();
        }

        protected void UpdateTagsFromText() {
            var tags = TagsTask.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(tag => tag.Trim().Replace("#", "").Replace(" ", "_"))
                               .ToArray();

            EditableTask.AppTaskTags = tags;
            TagsTask = string.Join(", ", tags);
        }
    }

}
