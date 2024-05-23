using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFG.Models {
    // Implementamos la interfaz IDisposable - "https://learn.microsoft.com/es-es/dotnet/api/system.idisposable?view=net-8.0"
    public class AppUser : IDisposable {
        private bool disposed = false; // Para detectar llamadas redundantes

        public AppUser() { }

        private ObjectId _appUserID;
        [BsonId]
        public ObjectId AppUserID {
            get { return _appUserID; }
            set { _appUserID = value; }
        }

        private string _appUserUsername;
        [BsonElement("aliasUsuario")]
        public required string AppUserUsername {
            get { return _appUserUsername; }
            set { _appUserUsername = value; }
        }
        private string _appUserEmail;
        [BsonElement("email")]
        public required string AppUserEmail {
            get { return _appUserEmail; }
            set { _appUserEmail = value; }
        }

        private string _appUserPassword;
        [BsonElement("password")]
        public required string AppUserPassword {
            get { return _appUserPassword; }
            set { _appUserPassword = value; }
        }
        private string _appUserName;
        [BsonElement("nombre")]
        public required string AppUserName {
            get { return _appUserName; }
            set { _appUserName = value; }
        }
        private string _appUserSurname1;
        [BsonElement("apellido1")]
        public required string AppUserSurname1 {
            get { return _appUserSurname1; }
            set { _appUserSurname1 = value; }
        }

        private string _appUserSurname2;
        [BsonElement("apellido2")]
        public string AppUserSurname2 {
            get { return _appUserSurname2; }
            set { _appUserSurname2 = value; }
        }
        private List<ObjectId> _appUserAppContainerList;
        [BsonElement("contenedores")]
        public List<ObjectId> AppUserAppContainerList {
            get { return _appUserAppContainerList; }
            set { _appUserAppContainerList = value; }
        }

        // Implementación del método Dispose, nos permite eliminar los datos en memoria.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Destructor
        ~AppUser() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _appUserID = ObjectId.Empty;
                    _appUserUsername = string.Empty;
                    _appUserEmail = string.Empty;
                    _appUserPassword = string.Empty;
                    _appUserName = string.Empty;
                    _appUserSurname1 = string.Empty;
                    _appUserSurname2 = string.Empty;
                    _appUserAppContainerList = [];
                }
                disposed = true;
            }
        }
    }
}
