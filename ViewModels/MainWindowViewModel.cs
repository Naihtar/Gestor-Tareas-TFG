using System.Windows;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        private bool _isValueTrue;

        public bool IsValueTrue {
            get { return _isValueTrue; }
            set {
                _isValueTrue = value;
                OnPropertyChanged(nameof(IsValueTrue));
            }
        }

        private readonly INavigationService _navigationService;
        private Visibility _navigationViewVisibility;

        public Visibility NavigationViewVisibility {
            get { return _navigationViewVisibility; }
            set {
                _navigationViewVisibility = value;
                OnPropertyChanged(nameof(NavigationViewVisibility));
            }
        }

        public MainWindowViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            NavigationViewVisibility = Visibility.Visible;
            // Navega a la página de inicio de sesión al iniciar
            _navigationService.NavigateToLogin(this);
        }
    }

}