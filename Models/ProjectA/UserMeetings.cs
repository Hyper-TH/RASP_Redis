using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace RASP_Redis.Models.ProjectA
{
    public class UserMeetings
    {
        [BsonId]
        public string? Id { get; set; }
        public string[] Meetings { get; set; }
    }
}
