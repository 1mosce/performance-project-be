using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class MethodologyService : IMethodologyService
    {
        private IMongoCollection<Methodology> _methodologies;

        public MethodologyService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _methodologies = database.GetCollection<Methodology>(settings.MethodologiesCollectionName);
        }
        public async Task<List<Methodology>> GetAsync()
        {
            return await _methodologies.Find(_ => true).ToListAsync();
        }

        public async Task<Methodology> GetAsync(ObjectId id)
        {
            return await _methodologies.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Methodology> CreateAsync(Methodology methodology)
        {
            await _methodologies.InsertOneAsync(methodology);
            return methodology;
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, Methodology methodology)
        {
            methodology.Id = id;
            await _methodologies.ReplaceOneAsync(m => m.Id == id, methodology);
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _methodologies.DeleteOneAsync(m => m.Id == id);
        }
    }
}
