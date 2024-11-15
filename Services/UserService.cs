﻿using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;

namespace PeopleManagmentSystem_API.Services
{
    public class UserService : IUserService
    {
        private IMongoCollection<User> _users; 
        private IMongoCollection<Company> _companies;

        public UserService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
        }
        public async Task<User> CreateAsync(User employee)
        {
            await _users.InsertOneAsync(employee);
            return employee;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _users.Find(u => true).ToListAsync();
        }

        public async Task<User?> GetAsync(ObjectId id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Company>> GetCompaniesAsync(ObjectId userId)
        {
            return await _companies.Find(c => c.UserIds.Contains(userId)).ToListAsync();
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, User user)
        {
            user.Id = id;
            await _users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async System.Threading.Tasks.Task AddPositionAsync(ObjectId userId, Position position)
        {
            var user = await GetAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id = {userId} not found.");
            }

            position.Id = ObjectId.GenerateNewId();
            user.Positions.Add(position);

            await UpdateAsync(userId, user);
        }

        public async System.Threading.Tasks.Task UpdatePositionAsync(ObjectId userId, ObjectId positionId, Position position)
        {
            var user = await GetAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id = {userId} not found.");
            }

            var existingPosition = user.Positions.FirstOrDefault(p => p.Id == positionId);
            if (existingPosition == null)
            {
                throw new KeyNotFoundException($"Position with Id = {positionId} not found.");
            }

            existingPosition.Name = position.Name;
            existingPosition.Description = position.Description;

            await UpdateAsync(userId, user);
        }

        public async System.Threading.Tasks.Task RemovePositionAsync(ObjectId userId, ObjectId positionId)
        {
            var user = await GetAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id = {userId} not found.");
            }

            var position = user.Positions.FirstOrDefault(p => p.Id == positionId);
            if (position == null)
            {
                throw new KeyNotFoundException($"Position with Id = {positionId} not found.");
            }

            user.Positions.Remove(position);

            await UpdateAsync(userId, user);
        }

    }
}
