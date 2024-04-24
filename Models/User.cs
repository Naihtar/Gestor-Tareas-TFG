using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFGDesktopApp.Models {
    public class User {

        private ObjectId _idUsuario;
        [BsonId]
        public ObjectId IdUsuario {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        private string _aliasUsuario;
        [BsonElement("aliasUsuario")]
        public required string AliasUsuario {
            get { return _aliasUsuario; }
            set { _aliasUsuario = value; }
        }
        private string _emailUsuario;
        [BsonElement("email")]
        public required string EmailUsuario {
            get { return _emailUsuario; }
            set { _emailUsuario = value; }
        }

        private string _passwordUsuario;
        [BsonElement("password")]
        public required string PasswordUsuario {
            get { return _passwordUsuario; }
            set { _passwordUsuario = value; }
        }
        private string _nombreUsuario;
        [BsonElement("nombre")]
        public required string NombreUsuario {
            get { return _nombreUsuario; }
            set { _nombreUsuario = value; }
        }
        private string _apellido1Usuario;
        [BsonElement("apellido1")]
        public required string Apellido1Usuario {
            get { return _apellido1Usuario; }
            set { _apellido1Usuario = value; }
        }

        private string _apellido2Usuario;
        [BsonElement("apellido2")]
        public required string Apellido2Usuario {
            get { return _apellido2Usuario; }
            set { _apellido2Usuario = value; }
        }
        private List<ObjectId>? _listaContenedoresUsuario;
        [BsonElement("contenedores")]
        public List<ObjectId>? ListaContenedoresUsuario {
            get { return _listaContenedoresUsuario; }
            set { _listaContenedoresUsuario = value; }
        }

        public User() { }
    } 
}
