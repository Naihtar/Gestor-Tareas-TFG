using TFG.Services.DatabaseServices;
using TFG.ViewModels.Base;
using TFG.ViewModels;
using TFGDesktopApp.Models;
using TFG.Services.NavigationServices;
namespace TFG.ViewModels {

    public class UserProfileEditViewModel : UserProfileBaseViewModel {
        public CommandViewModel SaveCommand { get; }
        public AppUser EditableUser { get; set; }

        public UserProfileEditViewModel(AppUser? user, NavigationService navigationService) : base(user, navigationService) {
            EditableUser = user ?? new AppUser() {
                Apellido2Usuario = string.Empty,
                AliasUsuario = string.Empty,
                EmailUsuario = string.Empty,
                PasswordUsuario = string.Empty,
                NombreUsuario = string.Empty,
                Apellido1Usuario = string.Empty
            };
            SaveCommand = new CommandViewModel(async obj => await SaveChangesAsync());
        }

        private async Task SaveChangesAsync() {
            // Actualiza el usuario en la base de datos
            await _databaseService.UpdateUserAsync(EditableUser);

            // Actualiza las propiedades del perfil de usuario
            UserData();

            // Navega hacia atrás
            _navigationService.NavigateTo("Profile", EditableUser, _navigationService);
        }
    }
}