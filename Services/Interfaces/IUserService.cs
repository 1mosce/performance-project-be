using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAsync();
        Task<User> GetAsync(ObjectId id);
        Task<List<Company>> GetCompaniesAsync(ObjectId id);
        Task<User> CreateAsync(User employee);
        Task UpdateAsync(ObjectId id, User employee);
        Task RemoveAsync(ObjectId id);

        // Positions
        Task AddPositionAsync(ObjectId userId, Position position);
        Task UpdatePositionAsync(ObjectId userId, ObjectId positionId, Position position);
        Task RemovePositionAsync(ObjectId userId, ObjectId positionId);
    }
}
