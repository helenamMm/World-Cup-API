using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldCupProjectApi.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")] public required string Nombre { get; set; }

        [BsonElement("apellido")] public required string Apellido { get; set; }

        [BsonElement("fecha_nacimiento")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public required DateTime FechaNacimiento { get; set; }

        [BsonElement("correo")] public required string Correo { get; set; }

        [BsonElement("contra")] public required string Contra { get; set; }

        [BsonElement("rol")] public string Rol { get; set; } = "user";

        [BsonElement("fecha_registro")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [BsonElement("activo")] public bool Activo { get; set; } = true; //Probablemente quitarlo aqui y mandarlo hasta el controller
        
        [BsonElement("favoritos")] 
        public Favoritos Favoritos { get; set; } = new Favoritos();
        
        public Usuario()
        {
            Favoritos = new Favoritos();
        }
    }
}

