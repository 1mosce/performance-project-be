using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        [BsonElement("company_name")]
        public string Name { get; set; } = String.Empty;
        [BsonElement("company_email")]
        public string Email { get; set; } = String.Empty;
    }
}
