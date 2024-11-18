using MongoDB.Bson;
using PerformanceProject.Client.Services.Generated;
using PerformanceProject.Shared.Models;
using System.Net.Http.Json;

namespace PerfomanceProject.Client.Services
{
    public class ProjectService
    {
        //private readonly HttpClient _httpClient;
        private readonly PerformanceProject.Client.Services.Generated.Client _client;
        //public ProjectService(HttpClient httpClient)
        public ProjectService(PerformanceProject.Client.Services.Generated.Client client)
        {
            //_httpClient = httpClient;
            _client = client;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            // var response = await _httpClient.GetFromJsonAsync<List<Project>>("https://localhost:44365/api/projects");
            // return response ?? new List<Project>();

            var response = await _client.Projects_GetAsync();
            return new List<Project>(response) ?? new List<Project>();
        }

        //public async Task<Project?> GetProjectByIdAsync(ObjectId projectId)
        //{
        //    return await _httpClient.GetFromJsonAsync<Project>($"api/projects/{projectId}");
        //}

        //public async Task<List<PerformanceProject.Shared.Models.Task>> GetTasksByProjectIdAsync(ObjectId projectId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PerformanceProject.Shared.Models.Task>>($"api/projects/{projectId}/tasks");
        //}
    }
}
