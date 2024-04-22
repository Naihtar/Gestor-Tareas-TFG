using MongoDB.Driver;
using System.Configuration;

namespace TFG.Database {
    class DatabaseConnection {
        // Atributos
        private readonly IMongoDatabase _database;

        // Constructor
        public DatabaseConnection() {
            var connectionString = ConnectionManager.GetConnectionString();
            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            _database = client.GetDatabase(databaseName);
        }

        // M�todo - Obtener una colecci�n de forma g�nerica.
        public IMongoCollection<T> GetCollection<T>(string collectionName) {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
