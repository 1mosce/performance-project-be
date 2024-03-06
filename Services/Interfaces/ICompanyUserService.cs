using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICompanyUserService
    {
        List<CompanyUser> Get();
        Company GetCompany(ObjectId id);
        User GetUser(ObjectId id);
        List<Company> GetUserCompanies(ObjectId id);
        List<User> GetCompanyUsers(ObjectId id);
        void Update(ObjectId companyId, ObjectId userId);
    }
}
