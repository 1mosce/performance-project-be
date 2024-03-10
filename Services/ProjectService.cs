using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class ProjectService : IProjectService
    {
        private IMongoCollection<Project> _companies;

        public ProjectService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companies = database.GetCollection<Project>(settings.ProjectCollectionName);
        }

        public Project Create(Project project)
        {
            _companies.InsertOne(project);
            return project;
        }

        public List<Project> Get()
        {
            return _companies.Find(p => true).ToList();
        }

        public Project Get(ObjectId id)
        {
            return _companies.Find(p => p.Id == id).FirstOrDefault();
        }

        public void Remove(ObjectId id)
        {
            _companies.DeleteOne(p => p.Id == id);
        }

        public void Update(ObjectId id, Project project)
        {
            _companies.ReplaceOne(p => p.Id == id, project);
        }
    }
}
