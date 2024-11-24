using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    public class Methodology
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [BsonIgnore]
        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = String.Empty;
       // public List<string> Phases { get; set; } = new List<string>();
    }
}
