using System.Globalization;
using System.Windows;
using System.Configuration;

namespace TFG.Services {
    public class LocalizationService : ILocalizationService {
        public void SetLanguage(string languageCode) {
            ResourceDictionary languageDictionary = [];
            //Esta estructura tan extraña me la recomendo el IDE, pero es un switch.
            languageDictionary.Source = languageCode switch {
                "es-ES" => new Uri("pack://application:,,,/Resources/Localization/Dictionary-es-ES.xaml", UriKind.RelativeOrAbsolute),
                "en-US" => new Uri("pack://application:,,,/Resources/Localization/Dictionary-en-US.xaml", UriKind.RelativeOrAbsolute),
                "fr-FR" => new Uri("pack://application:,,,/Resources/Localization/Dictionary-fr-FR.xaml", UriKind.RelativeOrAbsolute),
                _ => new Uri("pack://application:,,,/Resources/Localization/Dictionary-es-ES.xaml", UriKind.RelativeOrAbsolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(languageDictionary); //Aplicar el diccionario seleccionado

            Properties.Settings.Default.Language = languageCode; // Guarda el idioma seleccionado
            Properties.Settings.Default.Save(); // Guarda los cambios
        }

        public string GetLanguage() {
            return Properties.Settings.Default.Language; // Devuelve el idioma guardado
        }
    }
}
