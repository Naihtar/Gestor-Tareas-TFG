using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Views.Pages;
using TFG.Views.Pages.Access;
using TFG.Views.Pages.User;
using TFG.Views.Pages.Workspace.Container;
using TFG.Views.Pages.Workspace.Task;

namespace TFG.Services.NavigationServices {
    public class NavigationService(Frame frame, IDatabaseService databaseService, IAuthenticationService authenticationService) : INavigationService {
        private readonly Frame _frame = frame; //Marco principal, donde cargaran las vistas.
        private readonly IDatabaseService _databaseService = databaseService; //Dependencia de los servicios de la base de datos
        private readonly IAuthenticationService _authenticationService = authenticationService; //Dependencia de los servicios de autentificación

        public void GoBack() {
            //Volver para atrás
            if (_frame.CanGoBack) {
                _frame.GoBack();
            }
        }

        //Rutas:
        public void NavigateTo() {
            //Sign Up
            _frame.Navigate(new SignUpPage(_databaseService, _authenticationService, this));
        }

        public void NavigateTo(string? successMessage) {
            //Log In
            _frame.Navigate(new LogInPage(_authenticationService, this, successMessage));
        }


        // Editar Perfil y contraseña
        public void NavigateTo(string route, AppUser appUser) {
            switch (route) {
                case "ProfileEdit":
                    _frame.Navigate(new UserProfileEditPage(_databaseService, this, appUser: appUser));
                    break;
                case "ProfilePassword":
                    _frame.Navigate(new UserProfileEditPasswordPage(_databaseService, _authenticationService, this, appUser: appUser));
                    break;
                default:
                    _frame.Navigate(new UserProfileEditPage(_databaseService, this, appUser: appUser));
                    break;
            }
        }

        public void NavigateTo(string route, AppUser appUser, AppContainer? appContainer) {
            switch (route) {
                //Información de espacio de trabajo
                case "Container":
                    _frame.Navigate(new AppContainerPage(_databaseService, this, appUser: appUser, appContainer: appContainer));
                    break;
                //Editar el espacio de trabajo
                case "ContainerEdit":
                    _frame.Navigate(new AppContainerCreateOrEditPage(_databaseService, this, appUser: appUser, appContainer: appContainer));
                    break;
                //Añadir un nuevo espacio de trabajo
                case "ContainerAdd":
                    _frame.Navigate(new AppContainerCreateOrEditPage(_databaseService, this, appUser: appUser, appContainer: null));
                    break;
                default:
                    //En caso de error te retorna a la información del espacio de trabajo.
                    _frame.Navigate(new AppContainerPage(_databaseService, this, appUser: appUser, appContainer: appContainer));
                    break;
            }
        }

        public void NavigateTo(AppUser appUser, AppContainer? appContainer, AppTask? appTask) {
            //Eliminar el perfil de usuario
            _frame.Navigate(new UserProfileDeletePage(_databaseService, _authenticationService, this, appUser: appUser, appContainer: appContainer, appTask: appTask));
        }

        public void NavigateTo(AppUser appUser, AppContainer? appContainer, string? successMessage) {
            //Espacio de trabajo
            _frame.Navigate(new WorkSpacePage(_databaseService, this, appUser: appUser, appContainer: appContainer, successMessage));
        }

        public void NavigateTo(string route, AppUser appUser, AppContainer? appContainer, AppTask? appTask, string? data) {
            switch (route) {
                //Información sobre la tarea seleccionada
                case "Task":
                    _frame.Navigate(new AppTaskPage(_databaseService, this, appUser: appUser, appContainer: appContainer, appTask: appTask));
                    break;
                //Editar una tarea
                case "TaskEdit":
                    _frame.Navigate(new AppTaskCreateOrEditPage(_databaseService, this, appUser: appUser, appContainer: appContainer, appTask: appTask, null));
                    break;
                //Añadir una tarea
                case "TaskAdd":
                    _frame.Navigate(new AppTaskCreateOrEditPage(_databaseService, this, appUser: appUser, appContainer: appContainer, appTask: null, data));
                    break;
                //Información sobre el usuario
                case "Profile":
                    _frame.Navigate(new UserProfilePage(_databaseService, this, appUser: appUser, appContainer: appContainer, appTask: appTask, data));
                    break;
                default:
                    //En caso de error te retorna al perfil
                    _frame.Navigate(new UserProfilePage(_databaseService, this, appUser: appUser, appContainer: appContainer, appTask: appTask, data));
                    break;

            }
        }
    }
}

