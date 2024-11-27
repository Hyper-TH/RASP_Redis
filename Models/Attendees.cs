using MongoDB.Bson.Serialization.Attributes;

namespace RASP_Redis.Models
{
    public class Attendees
    {
        [BsonId]
        public string Id { get; set; }
        public string[] Users { get; set; } = null!;
    }
}
