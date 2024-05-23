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

        public UserProfileEditViewModel(AppUser? user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth, AppTask? task, AppContainer? container) : base(user, navigationService, db, auth) {
            SaveCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper());
            PasswordEditCommand = new CommandViewModel(EditPassword);
        }



        protected override async Task SaveChangesAsyncWrapper() {
            ObjectId idUser = EditableUser.AppUserID;
            bool email = await _databaseService.CheckUserByEmailAsync(idUser, EditableUser.AppUserEmail);
            bool alias = await _databaseService.CheckUserByUsernameAsync(idUser, EditableUser.AppUserUsername);
            bool fields = AreAnyFieldsEmpty();

            if (!IsValidEmail(EditableUser.AppUserEmail)) {
                ErrorMessage = "El email introducido no es válido.";
                return;
            }

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
            return string.IsNullOrEmpty(EditableUser.AppUserUsername) ||
                   string.IsNullOrEmpty(EditableUser.AppUserEmail) ||
                   string.IsNullOrEmpty(EditableUser.AppUserName) ||
                   string.IsNullOrEmpty(EditableUser.AppUserSurname1);
        }

        private void EditPassword(object obj) {
            _navigationService.NavigateTo("ProfilePassword", EditableUser, _navigationService, _databaseService, _authenticationService);
        }
    }
}