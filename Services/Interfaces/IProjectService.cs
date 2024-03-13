using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IProjectService
    {
        List<Project> Get();
        Project Get(ObjectId id); 
        List<Task> GetTasks(ObjectId id);
        Project Create(Project project);
        void Update(ObjectId id, Project project);
        void Remove(ObjectId id);
    }
}
