using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateAsync(Company company);
        Task<List<Company>> GetAsync(); 
        Task<Company> GetAsync(ObjectId id);
        Task<List<Project>> GetProjectsAsync(ObjectId id);
        Task<List<User>> GetUsersAsync(ObjectId id);
        Task<bool> UserExistsAsync(ObjectId userId);
        Task UpdateUserAsync(ObjectId companyId, ObjectId userId);
        Task UpdateAsync(ObjectId id, Company company);
        Task RemoveAsync(ObjectId id);
    }
}
