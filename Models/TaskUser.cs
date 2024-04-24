using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFG.Models {
    class TaskUser {
        private ObjectId _idTarea;
        [BsonId]
        public ObjectId IdTarea {
            get { return _idTarea; }
            set { _idTarea = value; }
        }

        private string _nombreTarea;
        [BsonElement("nombre")]
        public required string NombreTarea {
            get { return _nombreTarea; }
            set { _nombreTarea = value; }
        }

        [BsonElement("descripcion")]
        private string? _descriptionTarea;
        public string? DescripcionTarea {
            get { return _descriptionTarea; }
            set { _descriptionTarea = value; }
        }

        private DateTime _fechaCreacionTarea;
        [BsonElement("fechaCreacion")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaCreacionTarea {
            get { return _fechaCreacionTarea; }
            set { _fechaCreacionTarea = value; }
        }

        private DateTime _fechaModificacionTarea;
        [BsonElement("fechaModificacion")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaModificacionTarea {
            get { return _fechaModificacionTarea; }
            set { _fechaModificacionTarea = value; }
        }

        private DateTime _fechaVencimiento;
        [BsonElement("fechaVencimiento")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaVencimientoTarea {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }
        private string _estadoTarea;
        [BsonElement("estado")]
        public required string EstadoTarea {
            get { return _estadoTarea; }
            set { _estadoTarea = value; }
        }

        private string[] _etiquetasTarea = new string[3];
        [BsonElement("etiquetas")]
        public string[] EtiquetasTarea {
            get { return _etiquetasTarea; }
            set { _etiquetasTarea = value; }
        }
        private ObjectId _contenedorID;
        [BsonElement("contenedor_id")]
        public ObjectId ContenedorID {
            get { return _contenedorID; }
            set { _contenedorID = value; }
        }
    }
}