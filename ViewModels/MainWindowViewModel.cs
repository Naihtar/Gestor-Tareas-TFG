using TFG.Views.Pages;
using TFGDesktopApp.ViewModels;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {
        private readonly LoginPage _loginPage;
        private bool _isNavigationViewActive;

        public bool IsNavigationViewActive {
            get { return _isNavigationViewActive; }
            set {
                _isNavigationViewActive = value;
                OnPropertyChanged(nameof(IsNavigationViewActive));
            }
        }

        public MainWindowViewModel() {
            _loginPage = new LoginPage();
            IsNavigationViewActive = false;  // Asegúrate de que NavigationView no se muestre inicialmente
            MainWindowView.MFrame.Navigate(_loginPage);
        }
    }
}
