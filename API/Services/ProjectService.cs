using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
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

        public async Task<Project> GetAsync(string id)
        {
            var project = await _projects.Find(p => p.SerializedId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with SerializedId '{id}' not found.");
            }
            return project;
        }

        public async Task<List<PerformanceProject.Shared.Models.Task>> GetTasksAsync(string id)
        {
            var project = await _projects.Find(p => p.SerializedId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with SerializedId '{id}' not found.");
            }
            return project.Tasks;
        }

        public async Task RemoveAsync(string id)
        {
            await _projects.DeleteOneAsync(p => p.SerializedId == id);
        }

        public async Task UpdateAsync(string id, Project project)
        {
            project.Id = ObjectId.Parse(id);
            await _projects.ReplaceOneAsync(p => p.SerializedId == id, project);
        }

        // Methodology
        public async Task<Methodology?> GetMethodologyAsync(string projectId)
        {
            var project = await _projects.Find(p => p.SerializedId == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with SerializedId '{projectId}' not found.");

            return project.MainMethodology;
        }

        public async Task SetMethodologyAsync(string projectId, Methodology methodology)
        {
            var project = await _projects.Find(p => p.SerializedId == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with SerializedId '{projectId}' not found.");

            project.MainMethodology = methodology;
            await _projects.ReplaceOneAsync(p => p.SerializedId == projectId, project);
        }

        public async Task RemoveMethodologyAsync(string projectId)
        {
            var project = await _projects.Find(p => p.SerializedId == projectId).FirstOrDefaultAsync();

            if (project == null)
                throw new KeyNotFoundException($"Project with SerializedId '{projectId}' not found.");

            project.MainMethodology = null;
            await _projects.ReplaceOneAsync(p => p.SerializedId == projectId, project);
        }

        // Team
        public async Task<Team> GetTeamAsync(string projectId)
        {
            var project = await GetAsync(projectId);
            return project.Team;
        }

        public async Task SetTeamAsync(string projectId, Team team)
        {
            var project = await GetAsync(projectId);
            project.Team = team;
            await UpdateAsync(projectId, project);
        }

        public async Task RemoveTeamAsync(string projectId)
        {
            var project = await GetAsync(projectId);
            project.Team = null;
            await UpdateAsync(projectId, project);
        }

        // Tasks
        public async Task AddTaskAsync(string projectId, PerformanceProject.Shared.Models.Task task)
        {
            var project = await GetAsync(projectId);
            project.Tasks.Add(task);
            await UpdateAsync(projectId, project);
        }

        public async Task RemoveTaskAsync(string projectId, string taskId)
        {
            var project = await GetAsync(projectId);
            var task = project.Tasks.FirstOrDefault(t => t.SerializedId == taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with SerializedId '{taskId}' not found in project '{projectId}'.");
            }

            project.Tasks.Remove(task);
            await UpdateAsync(projectId, project);
        }

        public async Task UpdateTaskAsync(string projectId, PerformanceProject.Shared.Models.Task task)
        {
            var project = await GetAsync(projectId);
            var existingTask = project.Tasks.FirstOrDefault(t => t.SerializedId == task.SerializedId);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with SerializedId '{task.SerializedId}' not found in project '{projectId}'.");
            }

            project.Tasks.Remove(existingTask);
            project.Tasks.Add(task);
            await UpdateAsync(projectId, project);
        }

        // Status
        public async Task UpdateStatusAsync(string projectId, ProjectStatus status)
        {
            var project = await GetAsync(projectId);
            project.Status = status;
            await UpdateAsync(projectId, project);
        }

    }
}
