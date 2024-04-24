using System.Threading.Tasks;
using MongoDB.Driver;
using TFG.Database;

namespace TFG.Services.DatabaseServices {
    public class DatabaseService {
        private readonly DatabaseConnection _connection;

        public DatabaseService() {
            _connection = new DatabaseConnection();
        }

        public async Task<IMongoCollection<T>> GetCollectionAsync<T>(string collectionName) {
            return await Task.FromResult(_connection.GetCollection<T>(collectionName));
        }
    }
}
