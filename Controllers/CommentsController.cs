using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Comments")]
        public ActionResult<List<Comment>> Get()
        {
            return commentService.Get();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Comment by Id")]
        public ActionResult<Comment> Get(ObjectId id)
        {
            var comment = commentService.Get(id);

            if (comment == null)
            {
                return NotFound($"Comment with Id = {id} not found");
            }

            return comment;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Comment")]
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            commentService.Create(comment);

            return CreatedAtAction(nameof(Get), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Comment")]
        public ActionResult Put(ObjectId id, [FromBody] Comment comment)
        {
            var existingComment = commentService.Get(id);

            if (existingComment == null)
            {
                return NotFound($"Comment with Id = {id} not found");
            }

            commentService.Update(id, comment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Comment")]
        public ActionResult Delete(ObjectId id)
        {
            var comment = commentService.Get(id);

            if (comment == null)
            {
                return NotFound($"Comment with Id = {id} not found");
            }

            commentService.Remove(comment.Id);

            return Ok($"Comment with Id = {id} deleted");
        }
    }
}
