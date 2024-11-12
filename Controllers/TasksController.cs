using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Tasks")]
        public async Task<ActionResult<List<Task>>> Get()
        {
            var tasks = await taskService.GetAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Task by Id")]
        public async Task<ActionResult<Task>> Get(ObjectId id)
        {
            var task = await taskService.GetAsync(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return Ok(task);
        }

        [HttpGet("{id}/comments")]
        [SwaggerOperation(Summary = "Get Task's Comments")]
        public async Task<ActionResult<List<Comment>>> GetComments(ObjectId id)
        {
            var task = await taskService.GetAsync(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            var comments = await taskService.GetCommentsAsync(id);
            return Ok(comments);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Task")]
        public async Task<ActionResult<Task>> Post([FromBody] Task task)
        {
            await taskService.CreateAsync(task);
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Task")]
        public async Task<ActionResult> Put(ObjectId id, [FromBody] Task task)
        {
            var existingTask = await taskService.GetAsync(id);

            if (existingTask == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            await taskService.UpdateAsync(id, task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Task")]
        public async Task<ActionResult> Delete(ObjectId id)
        {
            var task = await taskService.GetAsync(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            await taskService.RemoveAsync(task.Id);

            return NoContent();
        }

        [HttpPost("{taskId}/comments")]
        [SwaggerOperation(Summary = "Add a Comment to a Task")]
        public async Task<IActionResult> AddComment(ObjectId taskId, [FromBody] Comment comment)
        {
            var task = await taskService.GetAsync(taskId);
            if (task == null)
            {
                return NotFound($"Task with Id = {taskId} not found");
            }

            comment.Id = ObjectId.GenerateNewId();
            comment.SentTime = DateTime.UtcNow;
            await taskService.AddCommentAsync(taskId, comment);

            return CreatedAtAction(nameof(Get), new { id = taskId }, comment);
        }

        [HttpPut("{taskId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Update a Comment in a Task")]
        public async Task<IActionResult> UpdateComment(ObjectId taskId, ObjectId commentId, [FromBody] string content)
        {
            var task = await taskService.GetAsync(taskId);
            if (task == null)
            {
                return NotFound($"Task with Id = {taskId} not found");
            }

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound($"Comment with Id = {commentId} not found in Task with Id = {taskId}");
            }
            comment.Content = content;

            await taskService.UpdateCommentAsync(taskId, commentId, content);

            return NoContent();
        }

        [HttpDelete("{taskId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Delete a Comment from a Task")]
        public async Task<IActionResult> DeleteComment(ObjectId taskId, ObjectId commentId)
        {
            var task = await taskService.GetAsync(taskId);
            if (task == null)
            {
                return NotFound($"Task with Id = {taskId} not found");
            }

            await taskService.RemoveCommentAsync(taskId, commentId);

            return NoContent();
        }

        //[HttpGet("{title}/skills")]
        //[SwaggerOperation(Summary = "Get Predicted Skills for Task")]
        //public ActionResult<string> GetSkills(string title)
        //{
        //    return taskService.GetSkills(title);
        //}

        //[HttpGet("{id}/productivity")]
        //[SwaggerOperation(Summary = "Get Task`s Productivity")]
        //public ActionResult<double> GetProductivity(ObjectId id)
        //{
        //    var task = taskService.Get(id);

        //    if (task == null)
        //    {
        //        return NotFound($"Task with Id = {id} not found");
        //    }

        //    return taskService.GetProductivity(id);
        //}
    }
}
