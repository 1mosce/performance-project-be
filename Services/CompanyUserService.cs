using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class CompanyUserService : ICompanyUserService
    {
        private IMongoCollection<Company> _companies;
        private IMongoCollection<User> _users;
        private IMongoCollection<CompanyUser> _companyUserRelation;

        public CompanyUserService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _companyUserRelation = database.GetCollection<CompanyUser>(settings.CompanyUserCollectionName);
        }

        public List<CompanyUser> Get()
        {
            return _companyUserRelation.Find(u => true).ToList();
        }

        [HttpGet("company/{id}")]
        public Company GetCompany(ObjectId id)
        {
            return _companies.Find(c => c.Id == id).FirstOrDefault();
        }

        [HttpGet("user/{id}")]
        public User GetUser(ObjectId id)
        {
            return _users.Find(u => u.Id == id).FirstOrDefault();
        }

        public List<Company> GetUserCompanies(ObjectId id)
        {
            var companiesIds = _companyUserRelation
                .Find(u => u.UserId == id.ToString())
                .Project(c => c.CompanyId)
                .ToList();

            return _companies.Find(u => companiesIds.Contains(u.Id.ToString())).ToList();
        }

        public List<User> GetCompanyUsers(ObjectId id)
        {
            var usersIds = _companyUserRelation
                 .Find(c => c.CompanyId == id.ToString())
                 .Project(u => u.UserId)
                 .ToList();

            return _users.Find(u => usersIds.Contains(u.Id.ToString())).ToList();
        }

        public void Update(ObjectId companyId, ObjectId userId)
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
    }
}
