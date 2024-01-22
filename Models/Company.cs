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
        [BsonElement("company_description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("company_employees")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> EmployeeIds { get; set; }

        [BsonElement("company_projects")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ProjectIds { get; set; }
    }
}
