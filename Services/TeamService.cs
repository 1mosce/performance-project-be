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
        private IMongoCollection<TeamUser> _teamUserRelation;

        public TeamService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _teams = database.GetCollection<Team>(settings.TeamCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _teamUserRelation = database.GetCollection<TeamUser>(settings.TeamUserCollectionName);
        }

        public Team Create(Team team)
        {
            _teams.InsertOne(team);
            return team;
        }

        public List<Team> Get()
        {
            return _teams.Find(t => true).ToList();
        }

        public Team Get(ObjectId id)
        {
            return _teams.Find(t => t.Id == id).FirstOrDefault();
        }

        public List<User> GetUsers(ObjectId id)
        {
            var usersIds = _teamUserRelation
                 .Find(t => t.TeamId == id.ToString())
                 .Project(u => u.UserId)
                 .ToList();

            return _users.Find(u => usersIds.Contains(u.Id.ToString())).ToList();
        }

        public void UpdateUser(ObjectId teamId, ObjectId userId, ObjectId teamRoleId)
        {
            var team = _teams.Find(t => t.Id == teamId).FirstOrDefault();
            var user = _users.Find(u => u.Id == userId).FirstOrDefault();
           // var teamRole = _users.Find(u => u.Id == userId).FirstOrDefault();

            if (team == null || user == null)
                return;

            var _teamId = teamId.ToString();
            var _userId = userId.ToString();
            var _teamRoleId = teamRoleId.ToString();

            var teamUserRelation = _teamUserRelation
            .Find(r => r.UserId == _userId && r.TeamId == _teamId)
            .SingleOrDefault();

            if (teamUserRelation == null)
                _teamUserRelation.InsertOne(new TeamUser() { TeamId = _teamId, UserId = _userId, TeamRoleId = _teamRoleId });
            else
                _teamUserRelation.DeleteOne(r => r.UserId == _userId && r.TeamId == _teamId);
        }

        public void Remove(ObjectId id)
        {
            _teams.DeleteOne(t => t.Id == id);
        }

        public void Update(ObjectId id, Team team)
        {
            team.Id = id;
            _teams.ReplaceOne(t => t.Id == id, team);
        }
    }
}
