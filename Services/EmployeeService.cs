using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;

namespace PeopleManagmentSystem_API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            _employees = database.GetCollection<Employee>(settings.EmployeeCollectionName);
        }

        public Employee Create(Employee employee)
        {
            _employees.InsertOne(employee);
            return employee;
        }

        public List<Employee> Get()
        {
            return _employees.Find(c => true).ToList();
        }

        public Employee Get(string id)
        {
            return _employees.Find(c => c.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _employees.DeleteOne(c => c.Id == id);
        }

        public void Update(string id, Employee employee)
        {
            _employees.ReplaceOne(c => c.Id == id, employee);
        }
    }
}
