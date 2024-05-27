using TFG.Models;

namespace TFG.Services.NavigationServices {

    public interface INavigationService {
        void GoBack();
        void NavigateTo();
        void NavigateTo(string? successMessage);
        void NavigateTo(string route, AppUser appUser);
        void NavigateTo(string route, AppUser appUser, AppContainer? appContainer);
        void NavigateTo(AppUser appUser, AppContainer? appContainer, AppTask? appTask);
        void NavigateTo(AppUser appUser, AppContainer? appContainer, string? successMessage);
        void NavigateTo(string route, AppUser appUser, AppContainer? appContainer, AppTask? appTask, string? data);
    }

}
