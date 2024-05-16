using MongoDB.Driver;
using System.Windows;

namespace TFG.Database {
    class DatabaseConnection : IDatabaseConnection {
        // Atributos
        private readonly IMongoDatabase? _database;
        private readonly IConnectionManager _connectionManager;

        // Constructor
        public DatabaseConnection(IConnectionManager connectionManager) {
            _connectionManager = connectionManager;
            try {
                var connectionString = _connectionManager.GetConnectionString();
                var client = new MongoClient(connectionString);
                var databaseName = new MongoUrl(connectionString).DatabaseName;
                _database = client.GetDatabase(databaseName);
            } catch (Exception ex) {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
                Application.Current.Shutdown();
            }
        }

        // Método - Obtener una colección de forma genérica.
        public IMongoCollection<T> GetCollection<T>(string collectionName) {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
