using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAsync();
        Task<User> GetAsync(string id);
        Task<List<Company>> GetCompaniesAsync(string id);
        Task<User> CreateAsync(User employee);
        Task UpdateAsync(string id, User employee);
        Task RemoveAsync(string id);

        // Positions
        Task AddPositionAsync(string userId, Position position);
        Task UpdatePositionAsync(string userId, string positionId, Position position);
        Task RemovePositionAsync(string userId, string positionId);
    }
}
