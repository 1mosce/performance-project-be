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

        public CompanyService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
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

        public async Task<Company> GetAsync(ObjectId id)
        {
            var company = await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }
            return company;
        }
        public async Task RemoveAsync(ObjectId id)
        {
            await _companies.DeleteOneAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(ObjectId id, Company company)
        {
            company.Id = id;
            await _companies.ReplaceOneAsync(c => c.Id == id, company);
        }

        // Users
        public async Task<List<User>> GetUsersByIdsAsync(List<ObjectId> userIds)
        {
            var filter = Builders<User>.Filter.In(u => u.Id, userIds); //проекція
            return await _users.Find(filter).ToListAsync();
        }

        public async Task<List<User>> GetUsersAsync(ObjectId id)
        {
            var company = await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }

            return await GetUsersByIdsAsync(company.UserIds);
        }

        public async Task<bool> UserExistsAsync(ObjectId userId)
        {
            return await _users.Find(user => user.Id == userId).AnyAsync();
        }
        public async Task AddUserAsync(ObjectId companyId, ObjectId userId)
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

        public async Task RemoveUserAsync(ObjectId companyId, ObjectId userId)
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
        public async Task<List<Project>> GetProjectsAsync(ObjectId id)
        {
            var company = await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }
            return company.Projects;
        }
        public async Task AddProjectAsync(ObjectId companyId, Project project)
        {
            var company = await GetAsync(companyId);
            company.Projects.Add(project);
            await UpdateAsync(companyId, company);
        }
        public async Task UpdateProjectAsync(ObjectId companyId, Project project)
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
        public async Task RemoveProjectAsync(ObjectId companyId, ObjectId projectId)
        {
            var company = await GetAsync(companyId);

            var project = company.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with Id = {projectId} not found in the company.");
            }

            company.Projects.Remove(project);
            await UpdateAsync(companyId, company);
        }
    }
}
