using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("roles")]
    public class Role : MongoIdentityRole<ObjectId>
    {

    }
}
