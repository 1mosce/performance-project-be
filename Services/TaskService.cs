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
        private IMongoCollection<Comment> _comments;

        public TaskService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
            _comments = database.GetCollection<Comment>(settings.CommentCollectionName);
        }

        //add ID existence check based on relationships
        public Task Create(Task task)
        {            
            _tasks.InsertOne(task);
            return task;
        }

        public List<Task> Get()
        {
            return _tasks.Find(t => true).ToList();
        }

        public Task Get(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).FirstOrDefault();
        }

        public double GetProductivity(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).First().CalculateProductivity();
        }

        public List<Comment> GetComments(ObjectId id)
        {
            _tasks.Find(c => c.Id == id).FirstOrDefault();

            return _comments
                .Find(c => c.TaskId == id.ToString())
                .ToList();
        }

        public void Remove(ObjectId id)
        {
            _tasks.DeleteOne(t => t.Id == id);
        }

        public void Update(ObjectId id, Task task)
        {
            task.Id = id;
            _tasks.ReplaceOne(t => t.Id == id, task);
        }
    }
}
