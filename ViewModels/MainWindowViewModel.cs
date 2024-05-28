using TFG.Services;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        //Dependencias
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService _localizationService;

        public MainWindowViewModel(INavigationService navigationService, ILocalizationService localizationService) {
            _navigationService = navigationService;
            _localizationService = localizationService;

            string languageCode = _localizationService.GetLanguage(); //Obtenemos el idioma por defecto de la app.
            _localizationService.SetLanguage(languageCode); // Cargar el idioma de la app

            _navigationService.NavigateTo(null);
        }
    }

}