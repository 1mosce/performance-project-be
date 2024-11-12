using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(Project project);
        Task<List<Project>> GetAsync();
        Task<Project> GetAsync(ObjectId id);
        Task<List<Task>> GetTasksAsync(ObjectId id);
        System.Threading.Tasks.Task UpdateAsync(ObjectId id, Project project);
        System.Threading.Tasks.Task RemoveAsync(ObjectId id);
    }
}
