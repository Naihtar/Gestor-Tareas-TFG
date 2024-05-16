using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Container;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages.Workspace.Container {
    /// <summary>
    /// Interaction logic for ContainerPage.xaml
    /// </summary>
    public partial class AppContainerPage : Page {
        public AppContainerPage(AppContainer container, INavigationService nav, AppUser user, IDatabaseService db, IAuthenticationService auth) {
            InitializeComponent();

            DataContext = new AppContainerViewModel(container, user, nav, db, auth);
        }
    }
}
