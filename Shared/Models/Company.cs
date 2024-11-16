using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("companies")]
    public class Company
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public List<ObjectId> UserIds { get; set; } = new(); 
        public List<Project> Projects { get; set; } = new();
    }
}
