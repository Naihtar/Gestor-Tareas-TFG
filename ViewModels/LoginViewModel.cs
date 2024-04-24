using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Views.Pages;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    class LoginViewModel {

        private readonly AuthenticationService _authenticationService;
        private readonly Frame _frame;
        private User? _user;
        private WorkSpacePage _t;

        public LoginViewModel() {
            _frame = MainWindowView.MFrame;
            _authenticationService = new AuthenticationService();
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password) {
            // Realiza la autenticación de forma asíncrona usando el servicio de autenticación
            if (await _authenticationService.AuthenticateUserAsync(username, password)) {
                // Si la autenticación es exitosa, devuelve el objeto User correspondiente
                _user = await _authenticationService.GetUserByUsernameAsync(username);
                return _user;
            } else {
                return null; // Devuelve null si la autenticación falla
            }
        }

        public void Test() {
            _t = new WorkSpacePage(_user);
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).IsNavigationViewActive = true;
            _frame.Navigate(_t);
        }

    }
}
