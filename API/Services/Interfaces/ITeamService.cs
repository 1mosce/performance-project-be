using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetAsync();
        Task<Team> GetAsync(ObjectId id);
        Task<Team> CreateAsync(Team team);
        Task UpdateAsync(ObjectId id, Team team);
        Task RemoveAsync(ObjectId id);

        //Members
        Task<List<TeamMember>> GetMembersAsync(ObjectId teamId);
        Task UpdateMemberAsync(ObjectId teamId, ObjectId userId, ObjectId teamRoleId);
        Task AddMemberAsync(ObjectId teamId, ObjectId userId, ObjectId teamRoleId);
        Task RemoveMemberAsync(ObjectId teamId, ObjectId userId);

        //Users
        Task<List<User>> GetUsersAsync(ObjectId id);
      
    }
}
