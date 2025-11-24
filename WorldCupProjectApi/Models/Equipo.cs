using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldCupProjectApi.Models;
public class Equipo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("nombre_completo_pais")]
    public string NombreCompletoPais { get; set; }

    [BsonElement("bandera")]
    public string Bandera { get; set; }

    [BsonElement("informacion")]
    public string Informacion { get; set; }

    [BsonElement("siglas_equipo")]
    public string SiglasEquipo { get; set; }

    [BsonElement("grupo")]
    public string Grupo { get; set; }

    [BsonElement("ranking_fifa")]
    public int RankingFifa { get; set; }

    [BsonElement("fecha_creacion")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [BsonElement("jugadores")]
    public List<Jugador> Jugadores { get; set; } = new List<Jugador>();
}