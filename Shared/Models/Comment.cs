using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("comments")]
    public class Comment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SerializedId { get => Id.ToString(); }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FromId { get; set; } = String.Empty;

        public required string Content { get; set; }

        public DateTime SentTime { get; set; }
    }

}
