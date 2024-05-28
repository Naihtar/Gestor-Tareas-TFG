using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;

namespace TFG.ViewModels.Workspace.Container { //Mostrar la información del espacio de trabajo
    class AppContainerViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer) : AppContainerBaseViewModel(databaseService, navigationService, appUser, appContainer) {
        protected override Task SaveContainerAsyncWrapper() {
            return Task.CompletedTask; //Para indicarle al programa que esta tarea se completo
        }
    };
}
