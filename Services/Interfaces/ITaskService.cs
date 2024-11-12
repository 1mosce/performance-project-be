using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<Task>> GetAsync();
        Task<Task> GetAsync(ObjectId id);
        Task<List<Comment>> GetCommentsAsync(ObjectId id);
        Task<Task> CreateAsync(Task task);
        System.Threading.Tasks.Task UpdateAsync(ObjectId id, Task task);
        System.Threading.Tasks.Task RemoveAsync(ObjectId id);
        System.Threading.Tasks.Task AddCommentAsync(ObjectId taskId, Comment comment);
        System.Threading.Tasks.Task UpdateCommentAsync(ObjectId taskId, ObjectId commentId, string content);
        System.Threading.Tasks.Task RemoveCommentAsync(ObjectId taskId, ObjectId commentId);

        //double GetProductivity(ObjectId id);
        //string GetSkills(string title);
    }
}
