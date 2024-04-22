using System.Threading.Tasks;
using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Views.Pages;

namespace TFG.ViewModels {
    class LoginViewModel {

        private readonly AuthenticationService _authenticationService;
        private readonly Frame _frame;

        public LoginViewModel() {
            _frame = MainWindowView.MFrame;
            _authenticationService = new AuthenticationService();
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password) {
            // Realiza la autenticación de forma asíncrona usando el servicio de autenticación
            return await _authenticationService.AuthenticateUserAsync(username, password);
        }

        public void Test() {
            Test t = new Test();
            _frame.Navigate(t);
        }
    }
}
