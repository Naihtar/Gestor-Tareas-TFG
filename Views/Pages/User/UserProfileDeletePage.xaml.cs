using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.User;

namespace TFG.Views.Pages.User {
    public partial class UserProfileDeletePage : Page {
        public UserProfileDeletePage(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService, AppUser appUser, AppContainer appContainer, AppTask appTask) {
            InitializeComponent();
            DataContext = new UserProfileDeleteViewModel(databaseService, authenticationService, navigationService, appUser, appContainer, appTask);
        }
    }
}
