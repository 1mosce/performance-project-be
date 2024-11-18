using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class PositionService : IPositionService
    {
        private IMongoCollection<Position> _positions;

        public PositionService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _positions = database.GetCollection<Position>(settings.TeamRolesCollectionName);
        }
        public async Task<List<Position>> GetAsync()
        {
            return await _positions.Find(_ => true).ToListAsync();
        }

        public async Task<Position?> GetAsync(string id)
        {
            return await _positions.Find(pos => pos.SerializedId == id).FirstOrDefaultAsync();
        }

        public async Task<Position> CreateAsync(Position position)
        {
            await _positions.InsertOneAsync(position);
            return position;
        }
        public async System.Threading.Tasks.Task UpdateAsync(string id, Position position)
        {
            position.Id = ObjectId.Parse(id);
            await _positions.ReplaceOneAsync(pos => pos.SerializedId == id, position);
        }

        public async System.Threading.Tasks.Task RemoveAsync(string id)
        {
            await _positions.DeleteOneAsync(pos => pos.SerializedId == id);
        }
    }
}
