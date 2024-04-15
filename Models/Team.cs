using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PeopleManagmentSystem_API.Models
{
    public class Team
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = string.Empty;
    }

}
