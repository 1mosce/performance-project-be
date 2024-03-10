using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICompanyService
    {
        List<Company> Get();
        Company Get(ObjectId id);
        List<User> GetUsers(ObjectId id);
        void UpdateUser(ObjectId companyId, ObjectId userId);
        List<Project> GetProjects(ObjectId id);
        Company Create(Company company); 
        void Update(ObjectId id, Company company);
        void Remove(ObjectId id);
    }
}
