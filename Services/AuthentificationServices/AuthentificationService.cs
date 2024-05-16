using System;
using System.Threading.Tasks;
using BCrypt.Net;
using MongoDB.Driver;
using TFG.Database;
using TFG.Services.DatabaseServices;
using TFGDesktopApp.Models;

namespace TFG.Services.AuthentificationServices {
    public class AuthenticationService(IDatabaseService db) : IAuthenticationService {

        private readonly IDatabaseService _databaseService = db;

        public async Task<AppUser> GetUserByDataInput(string input) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

            var filter = Builders<AppUser>.Filter.Or(
                            Builders<AppUser>.Filter.Eq(u => u.EmailUsuario, input),
                            Builders<AppUser>.Filter.Eq(u => u.AliasUsuario, input)
                        );

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(filter).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> AuthenticateUserAsync(string input, string password) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para buscar al usuario por email o alias
            var filter = Builders<AppUser>.Filter.Or(
                Builders<AppUser>.Filter.Eq(u => u.EmailUsuario, input),
                Builders<AppUser>.Filter.Eq(u => u.AliasUsuario, input)
            );

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(filter).FirstOrDefaultAsync();

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
