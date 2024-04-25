using System;
using System.Windows.Controls;
using System.Windows.Input;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Views.Pages;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {
        private readonly INavigationService _navigationService;

        public MainWindowViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
            // Navega a la página de inicio de sesión al iniciar
            _navigationService.NavigateToLogin();
        }
    }
}