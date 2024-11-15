using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<Models.Task>> GetAsync();
        Task<Models.Task> GetAsync(ObjectId id);
        Task<List<Comment>> GetCommentsAsync(ObjectId id);
        Task<Models.Task> CreateAsync(Models.Task task);
        Task UpdateAsync(ObjectId id, Models.Task task);
        Task RemoveAsync(ObjectId id);

        //Comments
        Task AddCommentAsync(ObjectId taskId, Comment comment);
        Task UpdateCommentAsync(ObjectId taskId, ObjectId commentId, string content);
        Task RemoveCommentAsync(ObjectId taskId, ObjectId commentId);

        // Assignee
        Task AssignUserAsync(ObjectId taskId, ObjectId userId);
        Task RemoveAssigneeAsync(ObjectId taskId);

        // Skills
        Task AddSkillAsync(ObjectId taskId, string skill);
        Task RemoveSkillAsync(ObjectId taskId, string skill);
        Task<List<string>> GetSkillsAsync(ObjectId taskId);

        // Status
        Task UpdateStatusAsync(ObjectId taskId, Models.TaskStatus status);

        // Difficulty
        Task UpdateDifficultyAsync(ObjectId taskId, DifficultyLevel difficulty);



        //double GetProductivity(ObjectId id);
        //string GetSkills(string title);
    }
}
