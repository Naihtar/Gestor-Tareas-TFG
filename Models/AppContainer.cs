using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFG.Models {
    public class AppContainer {
        public AppContainer() { }

        private ObjectId _idContenedor;
        [BsonId]
        public ObjectId IdContenedor {
            get { return _idContenedor; }

            set { _idContenedor = value; }
        }
        private string _nombreContenedor;

        [BsonElement("nombre")]
        public required string NombreContenedor {
            get { return _nombreContenedor; }
            set { _nombreContenedor = value; }
        }
        private string? _descripcionContenedor;
        [BsonElement("descripcion")]
        public string? DescripcionContenedor {
            get { return _descripcionContenedor; }
            set { _descripcionContenedor = value; }
        }

        private ObjectId _usuarioID;
        [BsonElement("usuario_id")]
        public ObjectId UsuarioID {
            get { return _usuarioID; }
            set { _usuarioID = value; }
        }

        private List<ObjectId>? _listaTareas;
        [BsonElement("tareas")]
        public List<ObjectId>? ListaTareas {
            get { return _listaTareas; }
            set { _listaTareas = value; }
        }

        private DateTime _fechaCreacionContenedor;
        [BsonElement("fechaCreacion")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaCreacionContenedor {
            get { return _fechaCreacionContenedor; }
            set { _fechaCreacionContenedor = value; }
        }

        //private DateTime _fechaAccesoContenedor;
        //[BsonElement("fechaAcceso")]
        //[BsonRepresentation(BsonType.DateTime)]
        //public DateTime FechaAccesoContenedor {
        //    get { return _fechaAccesoContenedor; }
        //    set { _fechaAccesoContenedor = value; }
        //}

        //private List<string> _listaEstadosTareas;
        //[BsonElement("estados")]
        //public required List<string> ListaEstadosTareas {
        //    get { return _listaEstadosTareas; }
        //    set { _listaEstadosTareas = value; }
        //}

    }
}
