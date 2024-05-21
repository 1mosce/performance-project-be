using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class ProjectService : IProjectService
    {
        private IMongoCollection<Project> _projects;
        private IMongoCollection<Task> _tasks;

        public ProjectService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _projects = database.GetCollection<Project>(settings.ProjectCollectionName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
        }

        public Project Create(Project project)
        {
            _projects.InsertOne(project);
            return project;
        }

        public List<Project> Get()
        {
            return _projects.Find(p => true).ToList();
        }

        public Project Get(ObjectId id)
        {
            return _projects.Find(p => p.Id == id).FirstOrDefault();
        }

        public List<Task> GetTasks(ObjectId id)
        {
            //_projects.Find(p => p.Id == id).FirstOrDefault();
            //var tas = _tasks
            //    .Find(t => t.ProjectId == id.ToString())
            //    .ToList();

            return _tasks.Find(t => true).ToList().FindAll(t => t.ProjectId == id.ToString());
        }

        public void Remove(ObjectId id)
        {
            _projects.DeleteOne(p => p.Id == id);
        }

        public void Update(ObjectId id, Project project)
        {
            project.Id = id;
            _projects.ReplaceOne(p => p.Id == id, project);
        }
    }
}
