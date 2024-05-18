using System.Windows;
using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LoginPage : Page {

        public LoginPage(INavigationService nav, IDatabaseService db, IAuthenticationService auth) {
            InitializeComponent();
            DataContext = new LoginViewModel(nav, db, auth);
        }

    }
}
