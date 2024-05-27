using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;
using System.Windows;

namespace TFG.ViewModels {
    class UserProfileViewModel : UserProfileBaseViewModel {

        //Atributos

        private readonly AppTask _appTask;
        private readonly AppContainer _appContainer;

        public CommandViewModel LogOutCommand { get; } //Cerrar sesión
        public CommandViewModel EditCommand { get; } //Editar el perfil
        public CommandViewModel DeleteCommand { get; } //Ir a la vista para eliminar la cuenta


        //Constructor
        public UserProfileViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask appTask, string? successMessage) : base(databaseService, navigationService, appUser) {
            _appTask = appTask;
            _appContainer = appContainer;
            Message = successMessage;

            LogOutCommand = new CommandViewModel(LogOut);
            EditCommand = new CommandViewModel(EditProfile);
            DeleteCommand = new CommandViewModel(DeleteProfile);

            if (Message != null) {
                ShowSuccessMessage(Message); //Cargar el mensaje de success
            }
        }
        private void LogOut(object obj) {
            _appUser?.Dispose(); //Vaciar de la memoria los datos del usuario
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
            _navigationService.NavigateTo(appUser: AppUserEditable, appContainer: _appContainer, appTask: _appTask);
        }

        protected override Task SaveChangesAsyncWrapper() {
            return Task.CompletedTask; //Tarea completada correctamente
        }
    }
}
