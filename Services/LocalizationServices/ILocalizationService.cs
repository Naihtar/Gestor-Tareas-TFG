namespace TFG.Services {
    public interface ILocalizationService {
        void SetLanguage(string languageCode);

        string GetLanguage();
    }
}
