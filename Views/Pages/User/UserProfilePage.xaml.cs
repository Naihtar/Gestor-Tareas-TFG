using System.Windows.Controls;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.Models;
using TFG.Services;

namespace TFG.Views.Pages {
    public partial class UserProfilePage : Page {
        public UserProfilePage(IDatabaseService databaseService, INavigationService navigationService,ILocalizationService localizationService ,AppUser? appUser, AppContainer? appContainer, AppTask? appTask, string? successMessage) {
            InitializeComponent();
            DataContext = new UserProfileViewModel(databaseService, navigationService, localizationService ,appUser: appUser, appContainer: appContainer, appTask: appTask, successMessage);
        }
    }
}
