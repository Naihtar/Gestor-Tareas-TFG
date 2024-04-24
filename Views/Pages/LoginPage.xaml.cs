using System.Windows;
using System.Windows.Controls;
using TFG.ViewModels;
using TFGDesktopApp.Models;

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
            User authenticatedUser = await _loginViewModel.AuthenticateUserAsync(LoginUser.Text, LoginPassword.Password);
            if (authenticatedUser != null) {
                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).IsNavigationViewActive = true;
                _mainFrame.Navigate(new WorkSpacePage(authenticatedUser));
            }

        }

    }
}
