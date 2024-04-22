using TFG.Views.Pages;

namespace TFG.ViewModels {
    public class MainWindowViewModel {
        private readonly LoginPage _loginPage;

        public MainWindowViewModel() {
            _loginPage = new LoginPage();

            MainWindowView.MFrame.Navigate(_loginPage);

        }

        
    }
}
