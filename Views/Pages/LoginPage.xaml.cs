using System.Windows;
using System.Windows.Controls;
using TFG.ViewModels;

namespace TFG.Views.Pages {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        private readonly Frame _mainFrame;
        private readonly LoginViewModel _loginViewModel;

        public LoginPage() {
            InitializeComponent();
            _loginViewModel = new LoginViewModel();
            _mainFrame = MainWindowView.MFrame;
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            if (await _loginViewModel.AuthenticateUserAsync(LoginUser.Text, LoginPassword.Password)) {
                _loginViewModel.Test();
            }
        }
    }
}
