using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Services
{
    public class CommentService : ICommentService
    {
        private IMongoCollection<Comment> _comments;

        public CommentService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _comments = database.GetCollection<Comment>(settings.CommentCollectionName);
        }

        //add ID existence check based on relationships
        public Comment Create(Comment comment)
        {            
            _comments.InsertOne(comment);
            return comment;
        }

        public List<Comment> Get()
        {
            return _comments.Find(c => true).ToList();
        }

        public Comment Get(ObjectId id)
        {
            return _comments.Find(c => c.Id == id).FirstOrDefault();
        }

        public void Remove(ObjectId id)
        {
            _comments.DeleteOne(c => c.Id == id);
        }

        public void Update(ObjectId id, Comment comment)
        {
            _comments.ReplaceOne(c => c.Id == id, comment);
        }
    }
}
