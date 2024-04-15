using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("team_user")]
    public class TeamUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string TeamId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string TeamRoleId { get; set; }
    }
}
