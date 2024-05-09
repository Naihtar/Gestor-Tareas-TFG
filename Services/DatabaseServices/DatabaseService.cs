using MongoDB.Bson;
using MongoDB.Driver;
using TFG.Database;
using TFG.Models;
using TFGDesktopApp.Models;

namespace TFG.Services.DatabaseServices {
    public class DatabaseService {
        private readonly DatabaseConnection _connection;

        public DatabaseService() {
            _connection = new DatabaseConnection();

        }

        public async Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName) {
            return await Task.FromResult(_connection.GetCollection<T>(collectionName));
        }

        public async Task<Container> GetContainerByIdAsync(ObjectId containerId) {
            var collection = await GetCollectionAsync<Container>("contenedores");
            var filter = Builders<Container>.Filter.Eq(c => c.IdContenedor, containerId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(ObjectId userId) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(u => u.IdUsuario == userId).FirstOrDefaultAsync();

            return user;
        }

        public async Task UpdateUserAsync(AppUser user) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para encontrar al usuario por su ID
            var filter = Builders<AppUser>.Filter.Eq(u => u.IdUsuario, user.IdUsuario);

            // Actualiza el usuario en la colección de forma asíncrona
            await users.ReplaceOneAsync(filter, user);
        }

        public async Task<bool> ExistEmailDB(ObjectId userId, string emailUsuario) {
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para buscar un usuario con el mismo email pero diferente al usuario actual
            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("email", emailUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );

            // Verifica si existe algún usuario que coincida con el filtro
            return await users.Find(filter).AnyAsync();
        }public async Task<bool> ExistAliasDB(ObjectId userId, string aliasUsuario) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");

            // Crea un filtro para buscar un usuario con el mismo email pero diferente al usuario actual
            var filter = Builders<AppUser>.Filter.And(
                Builders<AppUser>.Filter.Eq("aliasUsuario", aliasUsuario),
                Builders<AppUser>.Filter.Ne("_id", userId)
            );

            // Verifica si existe algún usuario que coincida con el filtro
            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> VerifyPasswordAsync(ObjectId userId, string password) {

            var users = await GetCollectionAsync<AppUser>("usuarios");

            var user = await users.Find(u => u.IdUsuario == userId).FirstOrDefaultAsync();

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordUsuario)) {
                return true;
            }

            return false;

        }


    }
}
