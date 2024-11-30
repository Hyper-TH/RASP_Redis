using MongoDB.Bson.Serialization.Attributes;

namespace RASP_Redis.Models.ProjectA
{
    public class Attendees
    {
        [BsonId]
        public string Id { get; set; }
        public string[] Users { get; set; } = null!;
    }
}
