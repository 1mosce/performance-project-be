using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetAsync();
        Task<Team> GetAsync(string id);
        Task<Team> CreateAsync(Team team);
        Task UpdateAsync(string id, Team team);
        Task RemoveAsync(string id);

        //Members
        Task<List<TeamMember>> GetMembersAsync(string teamId);
        Task UpdateMemberAsync(string teamId, string userId, string teamRoleId);
        Task AddMemberAsync(string teamId, string userId, string teamRoleId);
        Task RemoveMemberAsync(string teamId, string userId);

        //Users
        Task<List<User>> GetUsersAsync(string id);
      
    }
}
