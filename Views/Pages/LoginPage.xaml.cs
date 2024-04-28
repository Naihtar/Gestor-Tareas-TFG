using System.Windows;
using System.Windows.Controls;
using TFG.Services.NavigationServices;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    public partial class LoginPage : Page {
        private readonly LoginViewModel _loginViewModel;

        public LoginPage(Frame mainFrame, MainWindowViewModel mainWindowViewModel) {
            InitializeComponent();
            INavigationService navigationService = new NavigationService(mainFrame);
            _loginViewModel = new LoginViewModel(navigationService, mainWindowViewModel);
            DataContext = _loginViewModel;
        }
    }
}
