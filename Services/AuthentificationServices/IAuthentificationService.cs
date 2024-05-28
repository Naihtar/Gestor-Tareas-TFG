using TFG.Models;

namespace TFG.Services.AuthentificationServices {
    public interface IAuthenticationService {

        //Obtener el usuario por email o username.
        Task<AppUser?> GetUserByDataInputAsync(string input);

        //Comprobar que el usuario y la contraseña coinciden con los guardados en la base de datos.
        Task<bool> AuthenticateUserAsync(string input, string password);

        //Hashear la contraseña
        string HashPassword(string password);
    }
}
