using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("comments")]
    public class Comment
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [BsonIgnore]
        public string SerializedId { get => Id.ToString(); }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FromId { get; set; } = String.Empty;

        public required string Content { get; set; }

        public DateTime SentTime { get; set; }
    }

}
