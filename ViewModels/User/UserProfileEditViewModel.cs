using MongoDB.Bson;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;
using TFG.Models;


namespace TFG.ViewModels {
    public class UserProfileEditViewModel : UserProfileBaseViewModel {

        //Comandos
        public CommandViewModel SaveCommand { get; }
        public CommandViewModel PasswordEditCommand { get; }

        //Constructor
        public UserProfileEditViewModel(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser) : base(databaseService, navigationService, appUser) {
            SaveCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper());
            PasswordEditCommand = new CommandViewModel(EditPassword);
        }

        //Métodos
        protected override async Task SaveChangesAsyncWrapper() {
            //Comprueba que no quede ningún campo vacío
            bool fields = AreAnyFieldsEmpty();
            if (fields) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["EmptyFielStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            ObjectId idUser = AppUserEditable.AppUserID;
            //Comprueba que el email tenga una estructura válida
            if (!IsValidEmail(AppUserEditable.AppUserEmail)) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["ErrorEmailValidStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprobamos que el correo electrónico no existe en la base de datos
            bool email = await _databaseService.CheckUserByEmailAsync(idUser, AppUserEditable.AppUserEmail);
            if (email) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["EmailFieldMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            //Comprobamos que el nombre de usuario no existe en la base de datos
            bool alias = await _databaseService.CheckUserByUsernameAsync(idUser, AppUserEditable.AppUserUsername);
            if (alias) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["UserFieldMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            await SaveChangesAsync();
        }

        //Métodos auxiliares
        private bool AreAnyFieldsEmpty() {
            return string.IsNullOrEmpty(AppUserEditable.AppUserUsername) ||
                   string.IsNullOrEmpty(AppUserEditable.AppUserEmail) ||
                   string.IsNullOrEmpty(AppUserEditable.AppUserName) ||
                   string.IsNullOrEmpty(AppUserEditable.AppUserSurname1);
        }

        private void EditPassword(object obj) {
            _navigationService.NavigateTo("ProfilePassword", AppUserEditable);
        }
    }
}