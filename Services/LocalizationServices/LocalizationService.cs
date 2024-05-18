using System.Globalization;
using System.Windows;

namespace TFG.Services {
    public class LocalizationService {
        public void SetLang(string lang) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resdict = new ResourceDictionary() {
                Source = new Uri($"/Resources/Localization/Dictionary-{lang}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resdict);
        }
    }
}
