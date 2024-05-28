using System.Windows.Controls;
using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Tasks;

namespace TFG.Views.Pages.Workspace.Task {
    public partial class AppTaskCreateOrEditPage : Page {
        public AppTaskCreateOrEditPage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask? appTask, string? status) {
            InitializeComponent();
            DataContext = new AppTaskCreateOrEditViewModel(databaseService, navigationService, appUser, appContainer, appTask, status);
        }
    }
}
