using System.Windows.Controls;
using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Tasks;

namespace TFG.Views.Pages.Workspace.Task {
    public partial class AppTaskPage : Page {
        public AppTaskPage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask appTask) {
            InitializeComponent();
            DataContext = new AppTaskViewModel(databaseService, navigationService, appUser, appContainer, appTask);
        }
    }
}
