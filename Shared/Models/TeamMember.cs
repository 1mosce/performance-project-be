using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PerformanceProject.Shared.Models
{
    public class TeamMember
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamRoleId { get; set; } = string.Empty;
    }
}
