using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("team_role")]
    public class TeamRole
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [BsonIgnore]
        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
