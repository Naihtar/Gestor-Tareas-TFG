using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    /// <summary>
    /// Interaction logic for UserProfileEditPage.xaml
    /// </summary>
    public partial class UserProfileEditPage : Page
    {
        public UserProfileEditPage(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth)
        {
            InitializeComponent();
            DataContext = new UserProfileEditViewModel(user, nav, db, auth);
        }

    }
}
