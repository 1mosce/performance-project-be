using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
namespace PerformanceProject.Shared.Models
{
    [CollectionName("users")]
    public class User : MongoIdentityUser<string>
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public List<Position> Positions { get; set; } = new();
    }
}
