using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateAsync(Company company);
        Task<List<Company>> GetAsync(); 
        Task<Company> GetAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, Company company);
        Task RemoveAsync(ObjectId id);

        // Users
        Task<List<User>> GetUsersAsync(ObjectId id);
        Task<bool> UserExistsAsync(ObjectId userId);
        Task AddUserAsync(ObjectId companyId, ObjectId userId);
        Task RemoveUserAsync(ObjectId companyId, ObjectId userId);

        // Projects
        Task<List<Project>> GetProjectsAsync(ObjectId id);
        Task AddProjectAsync(ObjectId companyId, ObjectId projectId);
        Task UpdateProjectAsync(ObjectId companyId, Project project);
        Task RemoveProjectAsync(ObjectId companyId, ObjectId projectId);
    }
}
