using System.Threading.Tasks;
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

        public async Task<AppUser> GetUserByIdAsync(ObjectId id) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<AppUser>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(u => u.IdUsuario == id).FirstOrDefaultAsync();

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

        public async Task<bool> ExistNicknameDB(string aliasUsuario) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");
            var filterAlias = Builders<AppUser>.Filter.Eq(u => u.AliasUsuario, aliasUsuario);
            return await collection.Find(filterAlias).AnyAsync(); ;
        }
        public async Task<bool> ExistEmailDB(string emailUsuario) {
            var collection = await GetCollectionAsync<AppUser>("usuarios");
            var filterEmail = Builders<AppUser>.Filter.Eq(u => u.EmailUsuario, emailUsuario);
            return await collection.Find(filterEmail).AnyAsync();

        }
    }
}
