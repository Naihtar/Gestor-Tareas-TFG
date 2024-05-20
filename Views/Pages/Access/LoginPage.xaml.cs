using System.Windows;
using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LogInPage : Page {

        public LogInPage(INavigationService nav, IDatabaseService db, IAuthenticationService auth) {
            InitializeComponent();
            DataContext = new LogInViewModel(nav, db, auth);
        }

    }
}
