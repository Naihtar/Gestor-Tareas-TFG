using System;
using System.Threading.Tasks;
using BCrypt.Net;
using MongoDB.Driver;
using TFG.Database;
using TFG.Services.DatabaseServices;
using TFG.Models;

namespace TFG.Services.AuthentificationServices {
    public class AuthenticationService(IDatabaseService databaseService) : IAuthenticationService {

        private readonly IDatabaseService _databaseService = databaseService;

        // Obtener el usuario el email o correo.
        public async Task<AppUser?> GetUserByDataInputAsync(string input) {
            try {

                // Obtener la colección de usuarios.
                var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

                // Crear un filtro de búsqueda
                var filter = Builders<AppUser>.Filter.Or(
                                Builders<AppUser>.Filter.Eq(u => u.AppUserEmail, input),
                                Builders<AppUser>.Filter.Eq(u => u.AppUserUsername, input)
                            );

                // Busca al usuario en la colección usando el filtro.
                var user = await users.Find(filter).FirstOrDefaultAsync();

                return user; //Devolvemos el usuario encontrado.
            } catch (Exception) {
                //TODO - Implementar excepción
                return null;
            }
        }

        public async Task<bool> AuthenticateUserAsync(string input, string password) {
            try {

                // Obtener la colección de usuarios.
                var users = await _databaseService.GetCollectionAsync<AppUser>("usuarios");

                // Crear un filtro de búsqueda, para buscar al usuario por email o username
                var filter = Builders<AppUser>.Filter.Or(
                    Builders<AppUser>.Filter.Eq(u => u.AppUserEmail, input),
                    Builders<AppUser>.Filter.Eq(u => u.AppUserUsername, input)
                );

                // Busca al usuario en la colección usando el filtro.
                var user = await users.Find(filter).FirstOrDefaultAsync();

                // Si el usuario no existe, devolvera null y por tanto devuelve false
                if (user == null) {
                    return false;
                }

                // Verifica si la contraseña introducida coincide con la del usuario (usando bcrypt)
                return BCrypt.Net.BCrypt.Verify(password, user.AppUserPassword);
            } catch (Exception) {
                //TODO - Implementar excepción
                return false;
            }
        }


        public string HashPassword(string password) {
            // Hashea la contraseña usando bcrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
