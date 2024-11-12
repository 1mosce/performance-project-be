using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class TeamService : ITeamService
    {
        private IMongoCollection<Team> _teams;
        private IMongoCollection<User> _users;
        private IMongoCollection<TeamRole> _teamRoles;

        public TeamService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _teams = database.GetCollection<Team>(settings.TeamCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _teamRoles = database.GetCollection<TeamRole>(settings.TeamRolesCollectionName);
        }

        public async Task<Team> CreateAsync(Team team)
        {
            await _teams.InsertOneAsync(team);
            return team;
        }

        public async Task<List<Team>> GetAsync()
        {
            return await _teams.Find(t => true).ToListAsync();
        }

        public async Task<Team> GetAsync(ObjectId id)
        {
            return await _teams.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersAsync(ObjectId teamId)
        {
            var team = await _teams.Find(t => t.Id == teamId).FirstOrDefaultAsync();
            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found.");
            }

            var userIds = team.Members.Select(m => m.UserId).ToList();

            var users = await _users.Find(u => userIds.Contains(u.Id.ToString())).ToListAsync();

            return users;
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(ObjectId teamId, ObjectId userId, ObjectId teamRoleId)
        {
            var roleExists = await _teamRoles.Find(r => r.Id == teamRoleId).AnyAsync();

            if (!roleExists)
            {
                throw new KeyNotFoundException($"TeamRole with Id '{teamRoleId}' not found.");
            }

            var team = await _teams.Find(t => t.Id == teamId).FirstOrDefaultAsync();

            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found.");
            }

            var member = team.Members.FirstOrDefault(m => m.UserId == userId.ToString());

            if (member == null)
            {
                throw new KeyNotFoundException($"User with Id '{userId}' not found in Team with Id '{teamId}'.");
            }

            member.TeamRoleId = teamRoleId.ToString();

            var updateDefinition = Builders<Team>.Update.Set(t => t.Members, team.Members);
            await _teams.UpdateOneAsync(t => t.Id == teamId, updateDefinition);
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _teams.DeleteOneAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, Team team)
        {
            team.Id = id;
            await _teams.ReplaceOneAsync(t => t.Id == id, team);
        }
    }
}
