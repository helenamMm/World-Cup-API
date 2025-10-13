using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace WorldCupProjectApi.Models;

public class Partido
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("equipo_a")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EquipoAId { get; set; }

    [BsonElement("equipo_b")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EquipoBId { get; set; }

    [BsonElement("goles_equipo_a")]
    public int GolesEquipoA { get; set; } = 0;

    [BsonElement("goles_equipo_b")]
    public int GolesEquipoB { get; set; } = 0;

    [BsonElement("fecha")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Fecha { get; set; }

    [BsonElement("estadio")]
    public string Estadio { get; set; }

    [BsonElement("ciudad")]
    public string Ciudad { get; set; }

    [BsonElement("estado")]
    public string Estado { get; set; } = "PROGRAMADO"; // PROGRAMADO, EN_JUEGO, FINALIZADO, SUSPENDIDO

    [BsonElement("fase")]
    public string Fase { get; set; } // FASE_GRUPOS, OCTAVOS, CUARTOS, SEMIFINAL, FINAL

    [BsonElement("grupo")]
    public string Grupo { get; set; } // A, B, C, D, E, F, G, H (only for group phase)

    [BsonElement("arbitro_principal")]
    public string ArbitroPrincipal { get; set; }

    [BsonElement("fecha_creacion")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [BsonElement("fecha_actualizacion")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // Navigation properties (not stored in MongoDB)
    [BsonIgnore]
    public Equipo EquipoA { get; set; }

    [BsonIgnore]
    public Equipo EquipoB { get; set; }
}