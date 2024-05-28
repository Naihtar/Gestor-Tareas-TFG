using MongoDB.Driver;

namespace TFG.Database {
    public interface IDatabaseConnection {
        
        //Obtener una colleción de forma génerica.
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}

