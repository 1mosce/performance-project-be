using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;

namespace PeopleManagmentSystem_API.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _employees;

        public UserService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            _employees = database.GetCollection<User>(settings.UserCollectionName);
        }

        public User Create(User employee)
        {
            _employees.InsertOne(employee);
            return employee;
        }

        public List<User> Get()
        {
            return _employees.Find(c => true).ToList();
        }

        public User Get(ObjectId id)
        {
            return _employees.Find(c => c.Id == id).FirstOrDefault();
        }

        public void Remove(ObjectId id)
        {
            _employees.DeleteOne(c => c.Id == id);
        }

        public void Update(ObjectId id, User employee)
        {
            _employees.ReplaceOne(c => c.Id == id, employee);
        }
    }
}
