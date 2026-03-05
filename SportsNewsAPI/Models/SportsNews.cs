using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SportsNewsAPI.Models
{
    public class SportsNews
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("Content")]
        public string Content { get; set; } = string.Empty;

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Source")]
        public string Source { get; set; } = string.Empty;
    }
}
