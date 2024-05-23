using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.User {
    internal class UserProfileDeleteViewModel : UserProfileBaseViewModel {

        private readonly AppTask _task;
        private readonly AppContainer _container;
        public CommandViewModel DeleteProfileCommand { get; }

        private string? _emailUser;
        public string? EmailUser {

            get { return _emailUser; }
            set {
                _emailUser = value;
                OnPropertyChanged(nameof(EmailUser));
            }
        }

        private string? _passwordUser;
        public string? PasswordUser {
            get { return _passwordUser; }
            set {
                _passwordUser = value;
                OnPropertyChanged(nameof(PasswordUser));
            }
        }

        public UserProfileDeleteViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppTask task, AppContainer container) : base(user, nav, db, auth) {

            EmailUser = string.Empty;
            PasswordUser = string.Empty;
            _task = task;
            _container = container;
            DeleteProfileCommand = new CommandViewModel(async (obj) => await SaveChangesAsyncWrapper(), CanDelete);

        }

        protected override async Task SaveChangesAsyncWrapper() {
            AppUser? checkUser = await _databaseService.GetUserByEmailAsync(EmailUser);

            // Comprueba si checkUser es null
            if (checkUser == null) {
                ErrorMessage = "No se encontró ningún usuario con el email proporcionado.";
                return;
            }

            bool email = checkUser.AppUserEmail.Equals(EditableUser.AppUserEmail, StringComparison.CurrentCultureIgnoreCase);

            if (!email) {
                ErrorMessage = "El email introducido no es correcto.";
                return;
            }

            if (!await _databaseService.VerifyPasswordByUserIDAsync(_user.AppUserID, PasswordUser)) {
                ErrorMessage = "La contraseña introducida no es correcta.";
                return;
            }

            bool verifyUser = await _authenticationService.AuthenticateUserAsync(EmailUser, PasswordUser);

            if (!verifyUser) {
                ErrorMessage = "Datos introducidos incorrectos.";
                return;

            }
            bool success = await _databaseService.DeleteUserAsync(EditableUser);

            if (!success) {
                ErrorMessage = "Error al intentar eliminar el usuario";
                return;
            }
            _user?.Dispose();
            _task?.Dispose();
            _container?.Dispose();
            _navigationService.NavigateTo("LogIn", _databaseService, _authenticationService);
        }

        private bool CanDelete(object obj) {
            return !string.IsNullOrEmpty(EmailUser) && !string.IsNullOrEmpty(PasswordUser);
        }

    }
}
