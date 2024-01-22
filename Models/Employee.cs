using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using System.Security.Cryptography;
using System.Text;

namespace PeopleManagmentSystem_API.Models
{

    [CollectionName("employees")]
    public class Employee //: MongoIdentityUser<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        [BsonElement("employee_name")]
        public string Name { get; set; } = String.Empty;
        [BsonElement("employee_surname")]
        public string Surname { get; set; } = String.Empty;
        [BsonElement("employee_email")]
        public string Email { get; set; } = String.Empty;

        //hash
        [BsonElement("employee_password")]
        public string Password { get; set; } = String.Empty;


        [BsonElement("employee_phone")]
        public string Phone { get; set; } = String.Empty;

        [BsonElement("employee_roles")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public List<string> RoleIds { get; set; }

        [BsonElement("employee_positions")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]

        public List<string> PositionIds { get; set; }
    }
}
