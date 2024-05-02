using System.Windows;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        private readonly INavigationService _navigationService;
        public MainWindowViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            _navigationService.NavigateToLogin();
        }
    }

}