using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;

namespace PeopleManagmentSystem_API.Services
{
    public class UserService : IUserService
    {
        private IMongoCollection<User> _users; 
        private IMongoCollection<Company> _companies;
        private IMongoCollection<CompanyUser> _companyUserRelation;

        public UserService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
            _companyUserRelation = database.GetCollection<CompanyUser>(settings.CompanyUserCollectionName);
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

        public List<Company> GetCompanies(ObjectId id)
        {
            var companiesIds = _companyUserRelation
                .Find(u => u.UserId == id.ToString())
                .Project(c => c.CompanyId)
                .ToList();

            return _companies.Find(u => companiesIds.Contains(u.Id.ToString())).ToList();
        }

        public void Remove(ObjectId id)
        {
            _users.DeleteOne(u => u.Id == id);
        }

        public void Update(ObjectId id, User user)
        {
            user.Id = id;
            _users.ReplaceOne(u => u.Id == id, user);
        }
    }
}
