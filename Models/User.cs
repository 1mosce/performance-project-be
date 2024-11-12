using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("users")]
    public class User : MongoIdentityUser<ObjectId>
    {
        public string SerializedId { get => Id.ToString(); }
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public List<Position> Positions { get; set; } = new();
    }
}
