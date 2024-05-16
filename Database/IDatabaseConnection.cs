using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG.Database {
    public interface IDatabaseConnection {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}

