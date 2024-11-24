using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    public class Team
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [BsonIgnore]
        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = string.Empty;
        public List<TeamMember> Members { get; set; } = new List<TeamMember>();
    }
}
