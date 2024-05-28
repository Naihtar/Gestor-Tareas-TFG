namespace TFG.Database {
    class ConnectionManager : IConnectionManager {
        // Método - Obtener el string para conectarse
        public string GetConnectionString() {
            var connectionString = "mongodb+srv://abrahamrodriguez:1234@cmtaskmanagerdb.7lckevs.mongodb.net/TFG-BDD?retryWrites=true&w=majority";
            return connectionString;
        }
    }
}

