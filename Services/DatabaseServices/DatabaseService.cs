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
            // En lugar de simplemente devolver la colección, podrías envolverla en una tarea asíncrona.
            // Aquí, como GetCollection no es asincrónico, simplemente lo envolvemos en Task.FromResult.
            // Esto no hace que la operación sea realmente asincrónica, pero se adapta al modelo asíncrono.
            return await Task.FromResult(_connection.GetCollection<T>(collectionName));
        }
    }
}
