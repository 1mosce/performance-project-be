using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("roles")]
    public class Role : MongoIdentityRole<string>
    {

    }
}
