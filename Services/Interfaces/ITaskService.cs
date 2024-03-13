using MongoDB.Bson;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ITaskService
    {
        List<Task> Get();
        Task Get(ObjectId id);
        Task Create(Task project);
        void Update(ObjectId id, Task project);
        void Remove(ObjectId id);
    }
}
