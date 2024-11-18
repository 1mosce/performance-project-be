using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

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

        public async Task<Team> GetAsync(string id)
        {
            return await _teams.Find(t => t.SerializedId == id).FirstOrDefaultAsync();
        }
        public async Task<List<TeamMember>> GetMembersAsync(string teamId)
        {
            var team = await _teams.Find(t => t.SerializedId == teamId).FirstOrDefaultAsync();

            if (team == null)
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found");

            return team.Members;
        }
        public async Task<List<User>> GetUsersAsync(string teamId)
        {
            var team = await _teams.Find(t => t.SerializedId == teamId).FirstOrDefaultAsync();
            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found.");
            }

            var userIds = team.Members.Select(m => m.UserId).ToList();

            var users = await _users.Find(u => userIds.Contains(u.SerializedId.ToString())).ToListAsync();

            return users;
        }

        public async Task RemoveAsync(string id)
        {
            await _teams.DeleteOneAsync(t => t.SerializedId == id);
        }

        public async Task UpdateAsync(string id, Team team)
        {
            team.Id = ObjectId.Parse(id);
            await _teams.ReplaceOneAsync(t => t.SerializedId == id, team);
        }

        public async Task AddMemberAsync(string teamId, string userId, string teamRoleId)
        {
            var team = await _teams.Find(t => t.SerializedId == teamId).FirstOrDefaultAsync();

            if (team == null)
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found.");

            var userExists = await _users.Find(u => u.SerializedId == userId).AnyAsync();
            if (!userExists)
                throw new KeyNotFoundException($"User with Id '{userId}' not found.");

            var roleExists = await _teamRoles.Find(r => r.SerializedId == teamRoleId).AnyAsync();
            if (!roleExists)
                throw new KeyNotFoundException($"TeamRole with Id '{teamRoleId}' not found.");

            var newMember = new TeamMember
            {
                UserId = userId.ToString(),
                TeamRoleId = teamRoleId.ToString()
            };

            team.Members.Add(newMember);

            var updateDefinition = Builders<Team>.Update.Set(t => t.Members, team.Members);
            await _teams.UpdateOneAsync(t => t.SerializedId == teamId, updateDefinition);
        }

        public async Task RemoveMemberAsync(string teamId, string userId)
        {
            var team = await _teams.Find(t => t.SerializedId == teamId).FirstOrDefaultAsync();

            if (team == null)
                throw new KeyNotFoundException($"Team with Id '{teamId}' not found.");

            var member = team.Members.FirstOrDefault(m => m.UserId == userId.ToString());

            if (member == null)
            {
                throw new KeyNotFoundException($"User with Id '{userId}' not found in Team with Id '{teamId}'.");
            }

            team.Members.RemoveAll(m => m.UserId == userId.ToString());
            await _teams.ReplaceOneAsync(t => t.SerializedId == teamId, team);
        }

        public async Task UpdateMemberAsync(string teamId, string userId, string teamRoleId)
        {
            var roleExists = await _teamRoles.Find(r => r.SerializedId == teamRoleId).AnyAsync();

            if (!roleExists)
            {
                throw new KeyNotFoundException($"TeamRole with Id '{teamRoleId}' not found.");
            }

            var team = await _teams.Find(t => t.SerializedId == teamId).FirstOrDefaultAsync();

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
            await _teams.UpdateOneAsync(t => t.SerializedId == teamId, updateDefinition);
        }       
    }
}
