using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    /// <summary>
    /// Interaction logic for UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : Page
    {
        public UserProfilePage(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) {
            InitializeComponent();
            DataContext = new UserProfileViewModel(user, nav, db, auth);
        }
    }
}
