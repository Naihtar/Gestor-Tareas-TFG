using MongoDB.Driver;
using System.Configuration;
using System.Windows;

namespace TFG.Database {
    class DatabaseConnection {
        // Atributos
        private readonly IMongoDatabase? _database;

        // Constructor
        public DatabaseConnection() {
            try {
                var connectionString = ConnectionManager.GetConnectionString();
                var client = new MongoClient(connectionString);
                var databaseName = new MongoUrl(connectionString).DatabaseName;
                _database = client.GetDatabase(databaseName);
            } catch (Exception ex) {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
                Application.Current.Shutdown();
            }
        }

        // M�todo - Obtener una colecci�n de forma g�nerica.
        public IMongoCollection<T> GetCollection<T>(string collectionName) {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
