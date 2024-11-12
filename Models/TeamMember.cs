using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    public class TeamMember
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamRoleId { get; set; } = string.Empty;
    }
}
