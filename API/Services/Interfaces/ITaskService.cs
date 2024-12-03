using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<PerformanceProject.Shared.Models.Task>> GetAsync();
        Task<PerformanceProject.Shared.Models.Task> GetAsync(string id);
        Task<PerformanceProject.Shared.Models.Task> CreateAsync(PerformanceProject.Shared.Models.Task task);
        Task UpdateAsync(string id, PerformanceProject.Shared.Models.Task task);
        Task RemoveAsync(string id);

        //Comments
        Task<List<Comment>> GetCommentsAsync(string projectId, string taskId);
        Task<Comment> GetCommentAsync(string projectId, string taskId, string commentId);
        Task AddCommentAsync(string projectId, string taskId, Comment comment);
        Task UpdateCommentAsync(string projectId, string taskId, string commentId, string content);
        Task RemoveCommentAsync(string projectId, string taskId, string commentId);

        // Assignee
        Task AssignUserAsync(string projectId, string taskId, string userId);
        Task RemoveAssigneeAsync(string projectId, string taskId);

        // Skills
        Task AddSkillAsync(string projectId, string taskId, string skill);
        Task RemoveSkillAsync(string projectId, string taskId, string skill);
        Task<List<string>> GetSkillsAsync(string projectId, string taskId);

        // Status
        Task UpdateStatusAsync(string projectId, string taskId, PerformanceProject.Shared.Models.TaskStatus status);

        // Difficulty
        Task UpdateDifficultyAsync(string projectId, string taskId, DifficultyLevel difficulty);
    }
}
