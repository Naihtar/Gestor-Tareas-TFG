using MongoDB.Bson;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;


namespace TFG.ViewModels {
    public class UserProfileEditViewModel : UserProfileBaseViewModel {
        public CommandViewModel SaveCommand { get; }
        public CommandViewModel PasswordEditCommand { get; }

        public UserProfileEditViewModel(AppUser? user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) : base(user, navigationService, db, auth) {
            SaveCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper());
            PasswordEditCommand = new CommandViewModel(EditPassword);
        }



        protected override async Task SaveChangesAsyncWrapper() {
            ObjectId idUser = EditableUser.IdUsuario;
            bool email = await _databaseService.ExistEmailDB(idUser, EditableUser.EmailUsuario);
            bool alias = await _databaseService.ExistAliasDB(idUser, EditableUser.AliasUsuario);
            bool fields = AreAnyFieldsEmpty();


            if (fields) {
                ErrorMessage = "Rellene los campos vacios.";
                return;
            }
            if (email) {
                ErrorMessage = "El email introducido ya esta en uso.";
                return;
            }

            if (alias) {
                ErrorMessage = "El usuario introducido ya esta en uso.";
                return;
            }


            await SaveChangesAsync();
        }

        private bool AreAnyFieldsEmpty() {
            return string.IsNullOrEmpty(EditableUser.AliasUsuario) ||
                   string.IsNullOrEmpty(EditableUser.EmailUsuario) ||
                   string.IsNullOrEmpty(EditableUser.NombreUsuario) ||
                   string.IsNullOrEmpty(EditableUser.Apellido1Usuario) ||
                   string.IsNullOrEmpty(EditableUser.Apellido2Usuario);
        }

        private void EditPassword(object obj) {
            _navigationService.NavigateTo("ProfilePassword", EditableUser, _navigationService, _databaseService, _authenticationService);
        }
    }
}