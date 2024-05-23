using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.ViewModels.Base;
using TFG.Models;

namespace TFG.ViewModels {

    public class UserProfileEditPasswordViewModel : UserProfileBaseViewModel {

        public CommandViewModel PasswordSaveCommand { get; }


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


        public UserProfileEditPasswordViewModel(AppUser? user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth) : base(user, navigationService, db, auth) {

            OldPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordCheck = string.Empty;
            _authenticationService = auth;
            PasswordSaveCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper());
        }

        protected override async Task SaveChangesAsyncWrapper() {


            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPasswordCheck)) {
                ErrorMessage = "Rellene los campos vacios.";
                return;
            }

            //TODO
            if (!await _databaseService.VerifyPasswordByUserIDAsync(EditableUser.AppUserID, OldPassword)) {
                ErrorMessage = "La contraseña antigua es incorrecta.";
                return;
            }

            if (NewPassword != NewPasswordCheck) {
                ErrorMessage = "Las nuevas contraseñas no coinciden.";
                return;
            }

            EditableUser.AppUserPassword = _authenticationService.HashPassword(NewPassword);

            await SaveChangesAsync();
        }

    }
}