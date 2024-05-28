using MongoDB.Driver;
using System.Windows;

namespace TFG.Database {
    class DatabaseConnection : IDatabaseConnection {

        //Dependencias
        private readonly IMongoDatabase? _database;
        private readonly IConnectionManager _connectionManager;

        // Constructor
        public DatabaseConnection(IConnectionManager connectionManager) {
            _connectionManager = connectionManager;
            try {
                var connectionString = _connectionManager.GetConnectionString(); // Obtener la clave de conexi�n.
                var client = new MongoClient(connectionString); // Crear un cliente de MongoDB.
                var databaseName = new MongoUrl(connectionString).DatabaseName; // Extraer el nombre de la base de datos.
                _database = client.GetDatabase(databaseName); // Obtener una referencia a la base de datos especificada.
            } catch (Exception ex) {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}"); // Mostrar mensaje de error.
                Application.Current.Shutdown(); // Cerrar la aplicaci�n en caso de fallo.
            }
        }

        // Obtener una colecci�n de forma gen�rica.
        public IMongoCollection<T> GetCollection<T>(string collectionName) {
            if (_database == null) {
                throw new InvalidOperationException("Database connection is not initialized.");
            }
            return _database.GetCollection<T>(collectionName); // Devolver una colecci�n de la base de datos.
        }
    }
}
