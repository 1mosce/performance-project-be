using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("tasks")]
    public class Task
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }

        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssigneeId { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string StatusId { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string PriorityId { get; set; } = String.Empty;
        public DateTime DueDate { get; set; }
        //public TimeSpan TimeSpent { get; set; }
        //public double Rating { get; set; }
    }
}
