using MongoDB.Bson;
using PerformanceProject.Shared.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IMethodologyService
    {
        Task<List<Methodology>> GetAsync();
        Task<Methodology> GetAsync(string id);
        Task<Methodology> CreateAsync(Methodology methodology);
        System.Threading.Tasks.Task UpdateAsync(string id, Methodology methodology);
        System.Threading.Tasks.Task RemoveAsync(string id);
    }
}
