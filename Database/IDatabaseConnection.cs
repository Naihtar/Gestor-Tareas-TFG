using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG.Database {
    public interface IDatabaseConnection {
        
        //Obtener una colleción de forma génerica.
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}

