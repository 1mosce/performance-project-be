using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetAsync();
        Task<Team?> GetAsync(ObjectId id);
        Task<List<User>> GetUsersAsync(ObjectId id);
        Task UpdateUserAsync(ObjectId teamId, ObjectId userId, ObjectId teamRoleId);
        Task<Team> CreateAsync(Team team);
        Task UpdateAsync(ObjectId id, Team team);
        Task RemoveAsync(ObjectId id);
    }
}
