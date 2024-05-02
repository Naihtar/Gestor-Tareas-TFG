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

        public async Task<User> GetUserByIdAsync(ObjectId id) {
            // Obtén la colección de usuarios de forma asíncrona
            var users = await GetCollectionAsync<User>("usuarios");

            // Busca al usuario en la colección de forma asíncrona
            var user = await users.Find(u => u.IdUsuario == id).FirstOrDefaultAsync();

            return user;
        }
    }
}
