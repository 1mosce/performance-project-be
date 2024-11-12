using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetAsync();
        Task<Position?> GetAsync(ObjectId id);
        Task<Position> CreateAsync(Position position);
        System.Threading.Tasks.Task UpdateAsync(ObjectId id, Position position);
        System.Threading.Tasks.Task RemoveAsync(ObjectId id);
    }
}
