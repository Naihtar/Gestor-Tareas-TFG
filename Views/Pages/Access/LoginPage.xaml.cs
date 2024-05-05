using System.Windows;
using System.Windows.Controls;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LoginPage : Page {

        public LoginPage(Frame mainFrame) {
            InitializeComponent();
            INavigationService navigationService = new NavigationService(mainFrame);
            DataContext = new LoginViewModel(navigationService);
        }
    }
}
