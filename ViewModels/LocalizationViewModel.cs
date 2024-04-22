using System.Windows.Input;
using TFGDesktopApp.Services;
using TFGDesktopApp.ViewModels;

public class LocalizationViewModel : BaseViewModel {
    private LocalizationService _localizationService;

    public ICommand SetLanguageCommand { get; private set; }

    public bool IsEnglishButtonEnabled { get; private set; }
    public bool IsSpanishButtonEnabled { get; private set; }
    public bool IsFrenchButtonEnabled { get; private set; }

    public LocalizationViewModel(LocalizationService localizationService) {
        _localizationService = localizationService;
        SetLanguageCommand = new CommandViewModel(SetLang);

        // Establece el idioma predefinido
        SetLang("en-US");  // Cambia "en-US" al código de idioma que desees
    }

    private void SetLang(object langObj) {
        string? lang = langObj as string;
        _localizationService.SetLang(lang);

        // Actualiza las propiedades de los botones
        IsEnglishButtonEnabled = (lang != "en-US");
        IsSpanishButtonEnabled = (lang != "es-ES");
        IsFrenchButtonEnabled = (lang != "fr-FR");

        // Notifica a la vista que las propiedades han cambiado
        OnPropertyChanged(nameof(IsEnglishButtonEnabled));
        OnPropertyChanged(nameof(IsSpanishButtonEnabled));
        OnPropertyChanged(nameof(IsFrenchButtonEnabled));
    }
}
