using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class TeamRoleService : ITeamRoleService
    {
        private IMongoCollection<TeamRole> _teamRoles; 

        public TeamRoleService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _teamRoles = database.GetCollection<TeamRole>(settings.TeamRolesCollectionName);
        }
        public async Task<List<TeamRole>> GetAsync()
        {
            return await _teamRoles.Find(_ => true).ToListAsync();
        }

        public async Task<TeamRole?> GetAsync(ObjectId id)
        {
            return await _teamRoles.Find(role => role.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TeamRole> CreateAsync(TeamRole teamRole)
        {
            await _teamRoles.InsertOneAsync(teamRole);
            return teamRole;
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, TeamRole teamRole)
        {
            teamRole.Id = id;
            await _teamRoles.ReplaceOneAsync(role => role.Id == id, teamRole);
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _teamRoles.DeleteOneAsync(role => role.Id == id);
        }
    }
}
