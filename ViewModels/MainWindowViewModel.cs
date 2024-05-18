using System.Windows;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService _authenticationService;
        public MainWindowViewModel(INavigationService navigationService, IDatabaseService database, IAuthenticationService auth) {
            _navigationService = navigationService;
            _databaseService = database;
            _authenticationService = auth;
            _navigationService.NavigateTo(_databaseService, _authenticationService);
        }
    }

}