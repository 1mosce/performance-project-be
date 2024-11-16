using MongoDB.Bson;
using PerformanceProject.Shared.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamRoleService
    {
        Task<List<TeamRole>> GetAsync();
        Task<TeamRole?> GetAsync(ObjectId id);
        Task<TeamRole> CreateAsync(TeamRole teamRole);
        System.Threading.Tasks.Task UpdateAsync(ObjectId id, TeamRole teamRole);
        System.Threading.Tasks.Task RemoveAsync(ObjectId id);
    }
}
