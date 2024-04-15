using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services
{
    public interface IUserService
    {
        List<User> Get();
        User Get(ObjectId id);
        List<Company> GetCompanies(ObjectId id);
        User Create(User employee);
        void Update(ObjectId id, User employee);
        void Remove(ObjectId id);
    }
}
