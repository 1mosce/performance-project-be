using MongoDB.Bson;
using PerformanceProject.Shared.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IMethodologyService
    {
        Task<List<Methodology>> GetAsync();
        Task<Methodology> GetAsync(ObjectId id);
        Task<Methodology> CreateAsync(Methodology methodology);
        System.Threading.Tasks.Task UpdateAsync(ObjectId id, Methodology methodology);
        System.Threading.Tasks.Task RemoveAsync(ObjectId id);
    }
}
