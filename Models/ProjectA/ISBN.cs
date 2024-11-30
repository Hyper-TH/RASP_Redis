using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace RASP_Redis.Models.ProjectA
{
    public class ISBN
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Isbn { get; set; } = null!;
        public string docId { get; set; } = null!;
    }
}
