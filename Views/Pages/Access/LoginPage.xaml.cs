using System.Windows;
using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LogInPage : Page {

        public LogInPage(IAuthenticationService authenticationService, INavigationService navigationService, string? hasSuccessMessage) {
            InitializeComponent();
            DataContext = new LogInViewModel(authenticationService, navigationService, hasSuccessMessage);
        }

    }
}
