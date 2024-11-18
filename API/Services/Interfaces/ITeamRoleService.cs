using MongoDB.Bson;
using PerformanceProject.Shared.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamRoleService
    {
        Task<List<TeamRole>> GetAsync();
        Task<TeamRole?> GetAsync(string id);
        Task<TeamRole> CreateAsync(TeamRole teamRole);
        System.Threading.Tasks.Task UpdateAsync(string id, TeamRole teamRole);
        System.Threading.Tasks.Task RemoveAsync(string id);
    }
}
