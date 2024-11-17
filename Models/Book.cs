using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace RASP_Redis.Models
{
    public class Book
    {
        [BsonId]    // Document's primary key
        [BsonRepresentation(BsonType.ObjectId)] // Allow passing parameter as type string
        public string? Id { get; set; } // Required for mapping the CLR object to the collection

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BookName { get; set; } = null!;   // Property name
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;


    }
}
