using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.Models;

namespace TFG.Views.Pages {
    public partial class UserProfileEditPasswordPage : Page {
        public UserProfileEditPasswordPage(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService, AppUser appUser) {
            InitializeComponent();
            DataContext = new UserProfileEditPasswordViewModel(databaseService, authenticationService, navigationService, appUser);
        }
    }
}
