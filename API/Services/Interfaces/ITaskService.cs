using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<PerformanceProject.Shared.Models.Task>> GetAsync();
        Task<PerformanceProject.Shared.Models.Task> GetAsync(ObjectId id);
        Task<PerformanceProject.Shared.Models.Task> CreateAsync(PerformanceProject.Shared.Models.Task task);
        Task UpdateAsync(ObjectId id, PerformanceProject.Shared.Models.Task task);
        Task RemoveAsync(ObjectId id);

        //Comments
        Task<List<Comment>> GetCommentsAsync(ObjectId projectId, ObjectId taskId);
        Task<Comment> GetCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId);
        Task AddCommentAsync(ObjectId projectId, ObjectId taskId, Comment comment);
        Task UpdateCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId, string content);
        Task RemoveCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId);

        // Assignee
        Task AssignUserAsync(ObjectId projectId, ObjectId taskId, ObjectId userId);
        Task RemoveAssigneeAsync(ObjectId projectId, ObjectId taskId);

        // Skills
        Task AddSkillAsync(ObjectId projectId, ObjectId taskId, string skill);
        Task RemoveSkillAsync(ObjectId projectId, ObjectId taskId, string skill);
        Task<List<string>> GetSkillsAsync(ObjectId projectId, ObjectId taskId);

        // Status
        Task UpdateStatusAsync(ObjectId projectId, ObjectId taskId, PerformanceProject.Shared.Models.TaskStatus status);

        // Difficulty
        Task UpdateDifficultyAsync(ObjectId projectId, ObjectId taskId, DifficultyLevel difficulty);



        //double GetProductivity(ObjectId id);
        //string GetSkills(string title);
    }
}
