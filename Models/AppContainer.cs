using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFG.Models {
    public class AppContainer : IDisposable {
        private bool disposed = false;

        // Constructor
        public AppContainer() { }

        // Atributos
        private ObjectId _appContainerID;
        [BsonId]
        public ObjectId AppContainerID {
            get { return _appContainerID; }
            set { _appContainerID = value; }
        }

        private string _appContainerTitle;
        [BsonElement("nombre")]
        public string AppContainerTitle {
            get { return _appContainerTitle; }
            set { _appContainerTitle = value; }
        }

        private string? _appContainerDescription;
        [BsonElement("descripcion")]
        public string? AppContainerDescription {
            get { return _appContainerDescription; }
            set { _appContainerDescription = value; }
        }

        private ObjectId _appUserID;
        [BsonElement("usuario_id")]
        public ObjectId AppUserID {
            get { return _appUserID; }
            set { _appUserID = value; }
        }

        private List<ObjectId>? _appContainerAppTasksList;
        [BsonElement("tareas")]
        public List<ObjectId>? AppContainerAppTasksList {
            get { return _appContainerAppTasksList; }
            set { _appContainerAppTasksList = value; }
        }

        private DateTime _appContainerCreateDate;
        [BsonElement("fechaCreacion")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime AppContainerCreateDate {
            get { return _appContainerCreateDate; }
            set { _appContainerCreateDate = value; }
        }

        // Implementación del método Dispose, nos permite eliminar los datos en memoria.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Destructor
        ~AppContainer() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _appContainerID = ObjectId.Empty;
                    _appContainerTitle = string.Empty;
                    _appContainerDescription = string.Empty;
                    _appContainerAppTasksList = [];
                    _appContainerCreateDate = DateTime.MinValue;
                    _appUserID = ObjectId.Empty;
                }
                disposed = true;
            }
        }
    }
}
