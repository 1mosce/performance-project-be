using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;

namespace PeopleManagmentSystem_API.Services
{
    public class UserService : IUserService
    {
        private IMongoCollection<User> _users;

        public UserService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
        }

        public User Create(User employee)
        {
            _users.InsertOne(employee);
            return employee;
        }

        public List<User> Get()
        {
            return _users.Find(u => true).ToList();
        }

        public User Get(ObjectId id)
        {
            return _users.Find(u => u.Id == id).FirstOrDefault();
        }

        public void Remove(ObjectId id)
        {
            _users.DeleteOne(u => u.Id == id);
        }

        public void Update(ObjectId id, User employee)
        {
            _users.ReplaceOne(u => u.Id == id, employee);
        }
    }
}
