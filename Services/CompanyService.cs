using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class CompanyService : ICompanyService
    {
        private IMongoCollection<Company> _companies;
        private IMongoCollection<Project> _projects;
        private IMongoCollection<User> _users;
        private IMongoCollection<CompanyUser> _companyUserRelation;

        public CompanyService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _companyUserRelation = database.GetCollection<CompanyUser>(settings.CompanyUserCollectionName);
            _projects = database.GetCollection<Project>(settings.ProjectCollectionName);
        }

        public Company Create(Company company)
        {
            _companies.InsertOne(company);
            return company;
        }

        public List<Company> Get()
        {
            return _companies.Find(c => true).ToList();
        }

        public Company Get(ObjectId id)
        {
            return _companies.Find(c => c.Id == id).FirstOrDefault();
        }

        public List<User> GetUsers(ObjectId id)
        {
            var usersIds = _companyUserRelation
                 .Find(c => c.CompanyId == id.ToString())
                 .Project(u => u.UserId)
                 .ToList();

            return _users.Find(u => usersIds.Contains(u.Id.ToString())).ToList();
        }

        public void UpdateUser(ObjectId companyId, ObjectId userId)
        {
            var c = _companies.Find(c => true).ToList();
            var company = _companies.Find(c => c.Id == companyId).FirstOrDefault();
            var user = _users.Find(u => u.Id == userId).FirstOrDefault();

            if (company == null || user == null)
                return;

            var _companyId = companyId.ToString();
            var _userId = userId.ToString();

            var companyUserRelation = _companyUserRelation
            .Find(r => r.UserId == _userId && r.CompanyId == _companyId)
            .SingleOrDefault();

            if (companyUserRelation == null)
                _companyUserRelation.InsertOne(new CompanyUser() { CompanyId = _companyId, UserId = _userId });
            else
                _companyUserRelation.DeleteOne(r => r.UserId == _userId && r.CompanyId == _companyId);
        }

        public List<Project> GetProjects(ObjectId id)
        {
            _companies.Find(c => c.Id == id).FirstOrDefault();

            return _projects
                .Find(p => p.CompanyId == id.ToString())
                .ToList();
        }

        public void Remove(ObjectId id)
        {
            _companies.DeleteOne(c => c.Id == id);
        }

        public void Update(ObjectId id, Company company)
        {
            company.Id = id;
            _companies.ReplaceOne(c => c.Id == id, company);
        }
    }
}
