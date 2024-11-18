using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class CompanyService : ICompanyService
    {
        private IMongoCollection<Company> _companies;
        private IMongoCollection<User> _users;
        private readonly IProjectService _projectService;

        public CompanyService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient, IProjectService projectService)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _projectService = projectService;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            await _companies.InsertOneAsync(company);
            return company;
        }

        public async Task<List<Company>> GetAsync()
        {
            return await _companies.Find(c => true).ToListAsync();
        }

        public async Task<Company> GetAsync(string id)
        {
            var company = await _companies.Find(c => c.SerializedId == id).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }
            return company;
        }
        public async Task RemoveAsync(string id)
        {
            await _companies.DeleteOneAsync(c => c.SerializedId == id);
        }

        public async Task UpdateAsync(string id, Company company)
        {
            company.Id = ObjectId.Parse(id);
            await _companies.ReplaceOneAsync(c => c.SerializedId == id, company);
        }

        // Users
        public async Task<List<User>> GetUsersByIdsAsync(List<string> userIds)
        {
            var filter = Builders<User>.Filter.In(u => u.SerializedId, userIds); //проекція
            return await _users.Find(filter).ToListAsync();
        }

        public async Task<List<User>> GetUsersAsync(string id)
        {
            var company = await _companies.Find(c => c.SerializedId == id).FirstOrDefaultAsync();

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }

            return await GetUsersByIdsAsync(company.UserIds);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _users.Find(user => user.SerializedId == userId).AnyAsync();
        }
        public async Task AddUserAsync(string companyId, string userId)
        {
            var company = await GetAsync(companyId);

            if (company.UserIds.Contains(userId))
            {
                throw new InvalidOperationException($"User with Id = {userId} is already a member of the company.");
            }

            var userExists = await UserExistsAsync(userId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"User with Id = {userId} not found.");
            }

            company.UserIds.Add(userId);
            await UpdateAsync(companyId, company);
        }

        public async Task RemoveUserAsync(string companyId, string userId)
        {
            var company = await GetAsync(companyId);

            if (!company.UserIds.Contains(userId))
            {
                throw new InvalidOperationException($"User with Id = {userId} is not a member of the company.");
            }

            company.UserIds.Remove(userId);
            await UpdateAsync(companyId, company);
        }

        // Projects
        public async Task<List<Project>> GetProjectsAsync(string id)
        {
            var company = await _companies.Find(c => c.SerializedId == id).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }
            return company.Projects;
        }
        public async Task AddProjectAsync(string companyId, string projectId)
        {
            var company = await GetAsync(companyId);
            if (company == null)
                throw new KeyNotFoundException($"Company with ID {companyId} not found.");

            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            company.Projects.Add(project);
            await UpdateAsync(companyId, company);
        }
        public async Task UpdateProjectAsync(string companyId, Project project)
        {
            var company = await GetAsync(companyId);

            var existingProject = company.Projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException($"Project with Id = {project.Id} not found in the company.");
            }

            company.Projects.Remove(existingProject);
            company.Projects.Add(project);
            await UpdateAsync(companyId, company);
        }
        public async Task RemoveProjectAsync(string companyId, string projectId)
        {
            var company = await GetAsync(companyId);

            var project = company.Projects.FirstOrDefault(p => p.SerializedId == projectId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with Id = {projectId} not found in the company.");
            }

            company.Projects.Remove(project);
            await UpdateAsync(companyId, company);
        }
    }
}
