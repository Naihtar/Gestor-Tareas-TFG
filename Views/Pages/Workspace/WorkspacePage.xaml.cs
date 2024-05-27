using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class WorkSpacePage : Page {
        public WorkSpacePage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer? appContainer, string? successMessage) {
            InitializeComponent();
            InitializeViewModel(databaseService, navigationService, appUser, appContainer, successMessage);
        }

        private async void InitializeViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer? appContainer, string? successMessage) {
            DataContext = await WorkSpaceViewModel.CreateAsync(databaseService, navigationService, appUser, appContainer, successMessage);
        }
    }

}
