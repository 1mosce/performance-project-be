using MongoDB.Bson;
using PerformanceProject.Shared.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetAsync();
        Task<Position?> GetAsync(string id);
        Task<Position> CreateAsync(Position position);
        System.Threading.Tasks.Task UpdateAsync(string id, Position position);
        System.Threading.Tasks.Task RemoveAsync(string id);
    }
}
