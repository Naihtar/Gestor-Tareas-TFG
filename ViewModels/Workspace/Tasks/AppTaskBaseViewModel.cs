using System.Reflection.Metadata;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    public abstract class AppTaskBaseViewModel : BaseViewModel {

        public CommandViewModel WorkspaceCommand { get; }
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
                _tagsTask = value;
                OnPropertyChanged(TagsTask);
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
            WorkspaceCommand = new CommandViewModel(WorkspaceBack);
            SetStateCommand = new CommandViewModel(OnButtonPressed);
            EditableTask = _appTask ?? new AppTask() {

                NombreTarea = string.Empty,
                DescripcionTarea = string.Empty,
                FechaCreacionTarea = DateTime.Now,
                EstadoTarea = _statusTask,
                ContenedorID = _appContainer.IdContenedor,
                EtiquetasTarea = ["", "", ""], //TODO -> Revisar esta parte.

            };
            _tagsTask = string.Empty;
            _navigationService = navigationService;
            _databaseService = db;
            _authenticationService = auth;

            if (_appTask != null) {
                AppTaskData();
            }

        }

        protected async void AppTaskData() {
            AppTask t = await _databaseService.GetTastkByIdAsync(_appTask.IdTarea);
            TaskProperties = new Dictionary<string, string> {
                { "Nombre:", t.NombreTarea },
                { "Descripcion:", t.DescripcionTarea },
                { "Fecha:", t.FechaCreacionTarea.ToString() },
                { "Tags:", string.Join(", ", t.EtiquetasTarea.Select(etiqueta => $"#{etiqueta}")) }
            };
        }

        protected async Task SaveEditChangesAsync() {
            await _databaseService.UpdateTaskAsync(EditableTask);
            AppTaskData();

            _navigationService.NavigateTo("Task", _appContainer, _user, _navigationService, _databaseService, _authenticationService,
    EditableTask);
        }

        protected async Task SaveAddContainerAsync() {
            //TODO
        }

        private void OnButtonPressed(object obj) {
            string? buttonNumber = obj.ToString();

            _statusTask = buttonNumber;
        }

        protected abstract Task SaveTaskAsyncWrapper();

        private void WorkspaceBack(object obj) {

            _navigationService.GoBack();
        }

    }
}
