using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace RASP_Redis.Models
{
    public class User
    {
        [BsonId]    
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; } 

        public string UID { get; set; } = null!;

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;   // Property name
        public string Location {  get; set; }
    }
}
