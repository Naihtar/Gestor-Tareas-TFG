using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;
using TFG.Services;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {

        //Dependencias
        private readonly ILocalizationService _localizationService;

        //Atributos
        private readonly AppTask? _appTask;
        private readonly AppContainer? _appContainer;
        private readonly string _actualLanguage;

        public CommandViewModel LogOutCommand { get; } //Cerrar sesión
        public CommandViewModel EditCommand { get; } //Editar el perfil
        public CommandViewModel DeleteCommand { get; } //Ir a la vista para eliminar la cuenta

        public CommandViewModel ChangeLanguageCommand { get; } //Cargar el idioma seleccionado
        public CommandViewModel WorkspaceBackCommand { get; } //Volver atrás

        //Constructor
        public UserProfileViewModel(IDatabaseService databaseService, INavigationService navigationService, ILocalizationService localizationService, AppUser appUser, AppContainer? appContainer, AppTask? appTask, string? successMessage) : base(databaseService, navigationService, appUser) {

            _localizationService = localizationService;

            _appTask = appTask;
            _appContainer = appContainer;
            Message = successMessage;
            _actualLanguage = _localizationService.GetLanguage() ?? "es-ES";

            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            DeleteCommand = new CommandViewModel(DeleteProfile);
            ChangeLanguageCommand = new CommandViewModel(ChangeLanguage);
            WorkspaceBackCommand = new CommandViewModel(WorkspaceBack);

            DisableLanguageBtns(_actualLanguage);
            if (Message != null) {
                ShowSuccessMessage(Message); //Cargar el mensaje de success
            }
        }
        private void LogOut(object obj) {

            //Vaciar de la memoria los datos del usuario
            _appUser?.Dispose(); 
            AppUserEditable?.Dispose(); 
            UserProfileProperties.Clear(); 

            _appContainer?.Dispose(); //Vaciar de la memoria los datos del espacio de trabajo
            _appTask?.Dispose(); //Vaciar de la memoria los de la tarea



            //Mensaje de éxito
            string? message = ResourceDictionary["SuccessLogOutInfoBarStr"] as string;
            _navigationService.NavigateTo(message);
        }

        //Ir a la edición del perfil
        private void EditProfile(object obj) {
            _navigationService.NavigateTo("ProfileEdit", _appUser);
        }

        //Ir a la vista para eliminar el perfil
        private void DeleteProfile(object obj) {
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, appTask: _appTask);
        }

        private void ChangeLanguage(object obj) {

            string? lang = obj as string;

            lang ??= "es-ES"; //En caso de que status sea "null" se agregara automáticamente a "es-ES"

            DisableLanguageBtns(lang);

            _localizationService.SetLanguage(lang); //Mirar si cambiarlo por un boolean.

            string? msg = ResourceDictionary["SuccessLanguageChangeInfoBarStr"] as string;
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, msg);
        }

        protected override Task SaveChangesAsyncWrapper() {
            return Task.CompletedTask; //Tarea completada correctamente
        }

        private void WorkspaceBack(object obj) {
            _navigationService.NavigateTo(appUser: _appUser, appContainer: _appContainer, successMessage: null);
        }
    }
}
