using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace RASP_Redis.Models.ProjectA
{
    public class Meeting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string mID { get; set; } = null!;
        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;
        public string Organizer { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Date { get; set; } // Store only the date part (time will default to 00:00:00)
        public string Timezone { get; set; } = null!;
    }
}
