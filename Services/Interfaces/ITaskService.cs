using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        List<Task> Get();
        Task Get(ObjectId id);
        double GetProductivity(ObjectId id);
        List<Comment> GetComments(ObjectId id);
        Task Create(Task task);
        void Update(ObjectId id, Task task);
        void Remove(ObjectId id);
    }
}
