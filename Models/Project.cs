using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }

       public string Name { get; set; } = String.Empty;

       public string Description { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string CompanyId { get; set; } = String.Empty;

        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StatusId { get; set; } = String.Empty;

        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamId { get; set; } = String.Empty;
    }
}
