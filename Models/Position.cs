using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PeopleManagmentSystem_API.Models
{
    public class Position
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
