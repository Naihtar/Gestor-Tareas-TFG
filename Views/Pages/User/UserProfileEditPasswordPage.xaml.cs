using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.Models;

namespace TFG.Views.Pages {
    /// <summary>
    /// Interaction logic for UserProfileEditPasswordPage.xaml
    /// </summary>
    public partial class UserProfileEditPasswordPage : Page
    {
        public UserProfileEditPasswordPage(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth)
        {
            InitializeComponent();

            DataContext = new UserProfileEditPasswordViewModel(user, nav, db, auth);
        }
    }
}
