using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateAsync(Company company);
        Task<List<Company>> GetAsync(); 
        Task<Company> GetAsync(string id);
        Task UpdateAsync(string id, Company company);
        Task RemoveAsync(string id);

        // Users
        Task<List<User>> GetUsersAsync(string id);
        Task<bool> UserExistsAsync(string userId);
        Task AddUserAsync(string companyId, string userId);
        Task RemoveUserAsync(string companyId, string userId);

        // Projects
        Task<List<Project>> GetProjectsAsync(string id);
        Task AddProjectAsync(string companyId, string projectId);
        Task UpdateProjectAsync(string companyId, Project project);
        Task RemoveProjectAsync(string companyId, string projectId);
    }
}
