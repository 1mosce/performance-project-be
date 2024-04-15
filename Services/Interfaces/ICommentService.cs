using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface ICommentService
    {
        List<Comment> Get();
        Comment Get(ObjectId id);
        Comment Create(Comment comment);
        void Update(ObjectId id, Comment comment);
        void Remove(ObjectId id);
    }
}
