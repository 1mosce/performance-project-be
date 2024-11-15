using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

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

        public async Task<List<Models.Task>> GetTasksAsync(ObjectId id)
        {
            var project = await _projects.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with Id '{id}' not found.");
            }
            return project.Tasks;
        }

        public async Task RemoveAsync(ObjectId id)
        {
            await _projects.DeleteOneAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(ObjectId id, Project project)
        {
            project.Id = id;
            await _projects.ReplaceOneAsync(p => p.Id == id, project);
        }

        // Methodology
        public async Task<Methodology?> GetMethodologyAsync(ObjectId projectId)
        {
            var project = await _projects.Find(p => p.Id == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            return project.MainMethodology;
        }

        public async Task SetMethodologyAsync(ObjectId projectId, Methodology methodology)
        {
            var project = await _projects.Find(p => p.Id == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            project.MainMethodology = methodology;
            await _projects.ReplaceOneAsync(p => p.Id == projectId, project);
        }

        public async Task RemoveMethodologyAsync(ObjectId projectId)
        {
            var project = await _projects.Find(p => p.Id == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            project.MainMethodology = null;
            await _projects.ReplaceOneAsync(p => p.Id == projectId, project);
        }

        // Team
        public async Task<Team> GetTeamAsync(ObjectId projectId)
        {
            var project = await GetAsync(projectId);
            return project.Team;
        }

        public async Task SetTeamAsync(ObjectId projectId, Team team)
        {
            var project = await GetAsync(projectId);
            project.Team = team;
            await UpdateAsync(projectId, project);
        }

        public async Task RemoveTeamAsync(ObjectId projectId)
        {
            var project = await GetAsync(projectId);
            project.Team = null;
            await UpdateAsync(projectId, project);
        }

        // Tasks
        public async Task AddTaskAsync(ObjectId projectId, Models.Task task)
        {
            var project = await GetAsync(projectId);
            project.Tasks.Add(task);
            await UpdateAsync(projectId, project);
        }

        public async Task RemoveTaskAsync(ObjectId projectId, ObjectId taskId)
        {
            var project = await GetAsync(projectId);
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in project '{projectId}'.");
            }

            project.Tasks.Remove(task);
            await UpdateAsync(projectId, project);
        }

        public async Task UpdateTaskAsync(ObjectId projectId, Models.Task task)
        {
            var project = await GetAsync(projectId);
            var existingTask = project.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with Id '{task.Id}' not found in project '{projectId}'.");
            }

            project.Tasks.Remove(existingTask);
            project.Tasks.Add(task);
            await UpdateAsync(projectId, project);
        }

        // Status
        public async Task UpdateStatusAsync(ObjectId projectId, ProjectStatus status)
        {
            var project = await GetAsync(projectId);
            project.Status = status;
            await UpdateAsync(projectId, project);
        }

    }
}
