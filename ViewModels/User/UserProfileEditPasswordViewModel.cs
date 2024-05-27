using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {

    public class UserProfileEditPasswordViewModel : UserProfileBaseViewModel {

        private readonly IAuthenticationService _authenticationService; //Dependencia de los servicios de autentificación

        //Atributos
        private string? _oldPassword;
        public string? OldPassword {
            get { return _oldPassword; }
            set {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }
        private string? _newPassword;
        public string? NewPassword {
            get { return _newPassword; }
            set {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string? _newPasswordCheck;
        public string? NewPasswordCheck {
            get { return _newPasswordCheck; }
            set {
                _newPasswordCheck = value;
                OnPropertyChanged(nameof(NewPasswordCheck));
            }
        }

        //Comandos
        public CommandViewModel PasswordSaveCommand { get; } //Guardar cambios

        public UserProfileEditPasswordViewModel(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService, AppUser? appUser) : base(databaseService, navigationService, appUser) {
            _authenticationService = authenticationService;

            OldPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordCheck = string.Empty;

            PasswordSaveCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper());
        }

        //Método para guardar los cambios en la base de datos
        protected override async Task SaveChangesAsyncWrapper() {

            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPasswordCheck)) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["EmptyFielStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            bool password = await _databaseService.VerifyPasswordByUserIDAsync(AppUserEditable.AppUserID, OldPassword);
            if (!password) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["CheckPasswordFieldStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            if (NewPassword != NewPasswordCheck) {
                SuccessOpen = false;
                ErrorOpen = true;
                ErrorMessage = ResourceDictionary["PasswordMatchStr"] as string; //Mensaje de error
                StartTimer();
                return;
            }

            AppUserEditable.AppUserPassword = _authenticationService.HashPassword(NewPassword);
            await SaveChangesAsync();
        }

    }
}