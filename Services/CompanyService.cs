using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMongoCollection<Company> _companies;

        public CompanyService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
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

        public Company Get(string id)
        {
            return _companies.Find(c => c.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _companies.DeleteOne(c => c.Id == id);
        }

        public void Update(string id, Company company)
        {
            _companies.ReplaceOne(c => c.Id == id, company);
        }
    }
}
