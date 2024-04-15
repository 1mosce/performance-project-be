using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITeamService
    {
        List<Team> Get();
        Team Get(ObjectId id);
        List<User> GetUsers(ObjectId id);
        void UpdateUser(ObjectId teamId, ObjectId userId, ObjectId teamRoleId);
        Team Create(Team team); 
        void Update(ObjectId id, Team team);
        void Remove(ObjectId id);
    }
}
