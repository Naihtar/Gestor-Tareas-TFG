using System;
using System.Threading.Tasks;
using BCrypt.Net;
using MongoDB.Driver;
using TFG.Database;
using TFG.Services.DatabaseServices;
using TFGDesktopApp.Models;

namespace TFG.Services.AuthentificationServices {
    public class AuthenticationService {

        private readonly DatabaseService _databaseService;

        public AuthenticationService() {
            _databaseService = new DatabaseService();
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(u => u.AliasUsuario == username).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(u => u.AliasUsuario == username).FirstOrDefaultAsync();

            // Si el usuario no existe, devuelve false
            if (user == null) {
                return false;
            }
            // Verifica si la contraseña introducida coincide con la del usuario (usando bcrypt)
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordUsuario);
        }

        public string HashPassword(string password) {
            // Hashea la contraseña usando bcrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
