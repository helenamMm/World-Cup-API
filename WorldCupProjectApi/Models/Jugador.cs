using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldCupProjectApi.Models;
public class Jugador
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("apellido")]
    public string Apellido { get; set; }

    [BsonElement("fecha_nacimiento")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime FechaNacimiento { get; set; }

    [BsonElement("numero_camiseta")]
    public int NumeroCamiseta { get; set; }

    [BsonElement("posicion")]
    public string Posicion { get; set; }
}