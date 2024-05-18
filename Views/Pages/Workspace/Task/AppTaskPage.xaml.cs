using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Tasks;

namespace TFG.Views.Pages.Workspace.Task {
    /// <summary>
    /// Interaction logic for AppTaskPage.xaml
    /// </summary>
    public partial class AppTaskPage : Page {
        public AppTaskPage(AppContainer container, AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppTask task) {
            InitializeComponent();
            DataContext = new AppTaskViewModel(container, user, nav, db, auth, task);
        }
    }
}
