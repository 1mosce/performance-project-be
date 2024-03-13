using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class TaskService : ITaskService
    {
        private IMongoCollection<Task> _tasks;

        public TaskService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
        }

        //add ID existence check based on relationships
        public Task Create(Task task)
        {            
            _tasks.InsertOne(task);
            return task;
        }

        public List<Task> Get()
        {
            return _tasks.Find(p => true).ToList();
        }

        public Task Get(ObjectId id)
        {
            return _tasks.Find(p => p.Id == id).FirstOrDefault();
        }

        public void Remove(ObjectId id)
        {
            _tasks.DeleteOne(p => p.Id == id);
        }

        public void Update(ObjectId id, Task task)
        {
            _tasks.ReplaceOne(p => p.Id == id, task);
        }
    }
}
