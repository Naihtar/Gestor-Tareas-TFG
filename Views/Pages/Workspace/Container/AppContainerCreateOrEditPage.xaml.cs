using System.Windows.Controls;
using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Container;

namespace TFG.Views.Pages.Workspace.Container {
    public partial class AppContainerCreateOrEditPage : Page {
        public AppContainerCreateOrEditPage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer? appContainer) {
            InitializeComponent();
            DataContext = new AppContainerCreateOrEditViewModel(databaseService, navigationService, appUser, appContainer);
        }
    }
}
