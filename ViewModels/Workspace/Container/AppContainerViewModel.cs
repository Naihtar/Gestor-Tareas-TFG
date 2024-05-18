using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;

namespace TFG.ViewModels.Workspace.Container {
    class AppContainerViewModel(AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) : AppContainerBaseViewModel(container, user, navigationService, db, auth) {
        protected override Task SaveContainerAsyncWrapper() {
           return Task.CompletedTask;
        }
    };
}
