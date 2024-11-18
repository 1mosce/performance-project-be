using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(Project project);
        Task<List<Project>> GetAsync();
        Task<Project> GetAsync(string id);
        Task<List<PerformanceProject.Shared.Models.Task>> GetTasksAsync(string id);
        Task UpdateAsync(string id, Project project);
        Task RemoveAsync(string id);

        // Methodology
        Task<Methodology?> GetMethodologyAsync(string projectId);
        Task SetMethodologyAsync(string projectId, Methodology methodology);
        Task RemoveMethodologyAsync(string projectId);

        // Team
        Task<Team> GetTeamAsync(string projectId);
        Task SetTeamAsync(string projectId, Team team);
        Task RemoveTeamAsync(string projectId);

        // Tasks
        Task AddTaskAsync(string projectId, PerformanceProject.Shared.Models.Task task);
        Task RemoveTaskAsync(string projectId, string taskId);
        Task UpdateTaskAsync(string projectId, PerformanceProject.Shared.Models.Task task);

        // Status
        Task UpdateStatusAsync(string projectId, ProjectStatus status);

    }
}
