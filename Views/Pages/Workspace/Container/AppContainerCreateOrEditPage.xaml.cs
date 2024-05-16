using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Container;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages.Workspace.Container {
    /// <summary>
    /// Interaction logic for ContainerEditPage.xaml
    /// </summary>
    public partial class AppContainerCreateOrEditPage : Page
    {
        public AppContainerCreateOrEditPage(AppContainer container, AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth)
        {
            InitializeComponent();
            DataContext = new AppContainerCreateOrEditViewModel(container, user, nav, db, auth);
        }
    }
}
