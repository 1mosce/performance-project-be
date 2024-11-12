using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("team_role")]
    public class TeamRole
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
