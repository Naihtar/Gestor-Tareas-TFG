using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Tasks;

namespace TFG.Views.Pages.Workspace.Task {
    public partial class AppTaskEditPage : Page
    {
        public AppTaskEditPage(AppContainer container, AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppTask task) {
            InitializeComponent();
            DataContext = new AppTaskEditViewModel(task,container, user, nav, db, auth);
        }
    }
}
