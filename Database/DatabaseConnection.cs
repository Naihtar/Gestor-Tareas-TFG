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

        // Método - Obtener una colección de forma génerica.
        public IMongoCollection<T> GetCollection<T>(string collectionName) {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
