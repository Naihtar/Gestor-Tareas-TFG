using System.Windows;
using System.Windows.Controls;
using TFG.Services;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LogInPage : Page {

        public LogInPage(IAuthenticationService authenticationService, INavigationService navigationService, ILocalizationService localizationService ,string? hasSuccessMessage) {
            InitializeComponent();
            DataContext = new LogInViewModel(authenticationService, navigationService, localizationService, hasSuccessMessage);
        }
    }
}
