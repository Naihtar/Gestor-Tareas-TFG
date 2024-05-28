using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFG.Models {
    public class AppTask : IDisposable {
        private bool disposed = false;

        // Atributos
        private ObjectId _appTaskID;
        [BsonId]
        public ObjectId AppTaskID {
            get { return _appTaskID; }
            set { _appTaskID = value; }
        }

        private string? _appTaskTitle;
        [BsonElement("nombre")]
        public string? AppTaskTitle {
            get { return _appTaskTitle; }
            set { _appTaskTitle = value; }
        }

        private string? _appTaskDescription;
        [BsonElement("descripcion")]
        public string? AppTaskDescription {
            get { return _appTaskDescription; }
            set { _appTaskDescription = value; }
        }

        private DateTime _appTaskCreateDate;
        [BsonElement("fechaCreacion")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime AppTaskCreateDate {
            get { return _appTaskCreateDate; }
            set { _appTaskCreateDate = value; }
        }

        private string? _appTaskStatus;
        [BsonElement("estado")]
        public string? AppTaskStatus {
            get { return _appTaskStatus; }
            set { _appTaskStatus = value; }
        }

        private string[] _appTaskTags = new string[3];
        [BsonElement("etiquetas")]
        public string[] AppTaskTags {
            get { return _appTaskTags; }
            set { _appTaskTags = value; }
        }

        private ObjectId _appContainerID;
        [BsonElement("contenedor_id")]
        public ObjectId AppContainerID {
            get { return _appContainerID; }
            set { _appContainerID = value; }
        }

        // Implementación del método Dispose, nos permite eliminar los datos en memoria.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Destructor
        ~AppTask() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _appTaskID = ObjectId.Empty;
                    _appTaskTitle = string.Empty;
                    _appTaskDescription = string.Empty;
                    _appTaskCreateDate = DateTime.MinValue;
                    _appTaskStatus = string.Empty;
                    _appTaskTags = new string[3];
                    _appContainerID = ObjectId.Empty;
                }
                disposed = true;
            }
        }
    }
}
