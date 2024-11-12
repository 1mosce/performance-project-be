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

        public ProjectService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _projects = database.GetCollection<Project>(settings.ProjectCollectionName);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _projects.InsertOneAsync(project);
            return project;
        }

        public async Task<List<Project>> GetAsync()
        {
            return await _projects.Find(p => true).ToListAsync();
        }

        public async Task<Project> GetAsync(ObjectId id)
        {
            var project = await _projects.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with Id '{id}' not found.");
            }
            return project;
        }

        public async Task<List<Task>> GetTasksAsync(ObjectId id)
        {
            var project = await _projects.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with Id '{id}' not found.");
            }
            return project.Tasks;
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _projects.DeleteOneAsync(p => p.Id == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, Project project)
        {
            project.Id = id;
            await _projects.ReplaceOneAsync(p => p.Id == id, project);
        }
    }
}
