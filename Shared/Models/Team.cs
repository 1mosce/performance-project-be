using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PerformanceProject.Shared.Models
{
    public class Team
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = string.Empty;
        public List<TeamMember> Members { get; set; } = new List<TeamMember>();
    }
}
