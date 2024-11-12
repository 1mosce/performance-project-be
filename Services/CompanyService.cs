﻿using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
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

        public async Task UpdateUserAsync(ObjectId companyId, ObjectId userId)
        {
            var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);

            var company = await _companies.Find(filter).FirstOrDefaultAsync();

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{companyId}' not found.");
            }

            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id '{userId}' not found.");
            }

            var update = company.UserIds.Contains(userId)
        ? Builders<Company>.Update.Pull(c => c.UserIds, userId)
        : Builders<Company>.Update.Push(c => c.UserIds, userId);

            await _companies.UpdateOneAsync(filter, update);
        }

        public async Task<List<Project>> GetProjectsAsync(ObjectId id)
        {
            var company = await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new KeyNotFoundException($"Company with Id '{id}' not found.");
            }
            return company.Projects;
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
    }
}
