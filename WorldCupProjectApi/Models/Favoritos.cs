using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorldCupProjectApi.Models
{
    public class Favoritos
    {
        [BsonElement("partidos")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Partidos { get; set; } = new List<string>();

        [BsonElement("equipos")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Equipos { get; set; } = new List<string>();
    }
}