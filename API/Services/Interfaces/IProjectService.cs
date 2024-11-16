using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(Project project);
        Task<List<Project>> GetAsync();
        Task<Project> GetAsync(ObjectId id);
        Task<List<PerformanceProject.Shared.Models.Task>> GetTasksAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, Project project);
        Task RemoveAsync(ObjectId id);

        // Methodology
        Task<Methodology?> GetMethodologyAsync(ObjectId projectId);
        Task SetMethodologyAsync(ObjectId projectId, Methodology methodology);
        Task RemoveMethodologyAsync(ObjectId projectId);

        // Team
        Task<Team> GetTeamAsync(ObjectId projectId);
        Task SetTeamAsync(ObjectId projectId, Team team);
        Task RemoveTeamAsync(ObjectId projectId);

        // Tasks
        Task AddTaskAsync(ObjectId projectId, PerformanceProject.Shared.Models.Task task);
        Task RemoveTaskAsync(ObjectId projectId, ObjectId taskId);
        Task UpdateTaskAsync(ObjectId projectId, PerformanceProject.Shared.Models.Task task);

        // Status
        Task UpdateStatusAsync(ObjectId projectId, ProjectStatus status);

    }
}
