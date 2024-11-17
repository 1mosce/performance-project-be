using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

using Task = PerformanceProject.Shared.Models.Task;

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
        public async Task<ActionResult<Task>> GetById(ObjectId id)
        {
            var task = await taskService.GetAsync(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return Ok(task);
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

        [HttpGet("{projectId}/tasks/{taskId}/comments")]
        [SwaggerOperation(Summary = "Get All Comments from a Task in Project")]
        public async Task<ActionResult<List<Comment>>> GetComments(ObjectId projectId, ObjectId taskId)
        {
            var comments = await taskService.GetCommentsAsync(projectId, taskId);
            return Ok(comments);
        }

        [HttpGet("{projectId}/tasks/{taskId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Get a Comment from a Task in Project")]
        public async Task<ActionResult<Comment>> GetComment(ObjectId projectId, ObjectId taskId, ObjectId commentId)
        {
            var comment = await taskService.GetCommentAsync(projectId, taskId, commentId);
            return Ok(comment);
        }

        [HttpPost("{projectId}/tasks/{taskId}/comments")]
        [SwaggerOperation(Summary = "Add a Comment to a Task in Project")]
        public async Task<IActionResult> AddComment(ObjectId projectId, ObjectId taskId, [FromBody] Comment comment)
        {
            await taskService.AddCommentAsync(projectId, taskId, comment);
            return CreatedAtAction(nameof(GetComments), new { projectId, taskId }, comment);
        }

        [HttpPut("{projectId}/tasks/{taskId}/comments/{commentId}")]

        [SwaggerOperation(Summary = "Update a Comment from a Task in Project")]
        public async Task<IActionResult> UpdateComment(ObjectId projectId, ObjectId taskId, ObjectId commentId, [FromBody] string content)
        {
            await taskService.UpdateCommentAsync(projectId, taskId, commentId, content);
            return NoContent();
        }

        [HttpDelete("{projectId}/tasks/{taskId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Remove a Comment from a Task in Project")]
        public async Task<IActionResult> RemoveComment(ObjectId projectId, ObjectId taskId, ObjectId commentId)
        {
            await taskService.RemoveCommentAsync(projectId, taskId, commentId);
            return NoContent();
        }
        // Assign user
        [HttpPut("project/{projectId}/task/{taskId}/assign/{userId}")]
        [SwaggerOperation(Summary = "Assign User to Task")]
        public async Task<IActionResult> AssignUser(ObjectId projectId, ObjectId taskId, ObjectId userId)
        {
            await taskService.AssignUserAsync(projectId, taskId, userId);
            return NoContent();
        }

        // Remove assignee
        [HttpPut("project/{projectId}/task/{taskId}/unassign")]
        [SwaggerOperation(Summary = "Remove Assignee from Task")]
        public async Task<IActionResult> RemoveAssignee(ObjectId projectId, ObjectId taskId)
        {
            await taskService.RemoveAssigneeAsync(projectId, taskId);
            return NoContent();
        }

        // Add skill
        [HttpPost("project/{projectId}/task/{taskId}/skills")]
        [SwaggerOperation(Summary = "Add Skill to Task")]
        public async Task<IActionResult> AddSkill(ObjectId projectId, ObjectId taskId, [FromBody] string skill)
        {
            await taskService.AddSkillAsync(projectId, taskId, skill);
            return NoContent();
        }

        // Remove skill
        [HttpDelete("project/{projectId}/task/{taskId}/skills")]
        [SwaggerOperation(Summary = "Remove Skill from Task")]
        public async Task<IActionResult> RemoveSkill(ObjectId projectId, ObjectId taskId, [FromBody] string skill)
        {
            await taskService.RemoveSkillAsync(projectId, taskId, skill);
            return NoContent();
        }

        // Get skills
        [HttpGet("project/{projectId}/task/{taskId}/skills")]
        [SwaggerOperation(Summary = "Get Task Skills")]
        public async Task<ActionResult<List<string>>> GetSkills(ObjectId projectId, ObjectId taskId)
        {
            var skills = await taskService.GetSkillsAsync(projectId, taskId);
            return Ok(skills);
        }

        // Update status
        [HttpPut("project/{projectId}/task/{taskId}/status")]
        [SwaggerOperation(Summary = "Update Task Status")]
        public async Task<IActionResult> UpdateStatus(ObjectId projectId, ObjectId taskId, [FromBody] PerformanceProject.Shared.Models.TaskStatus status)
        {
            await taskService.UpdateStatusAsync(projectId, taskId, status);
            return NoContent();
        }

        // Update difficulty
        [HttpPut("project/{projectId}/task/{taskId}/difficulty")]
        [SwaggerOperation(Summary = "Update Task Difficulty")]
        public async Task<IActionResult> UpdateDifficulty(ObjectId projectId, ObjectId taskId, [FromBody] DifficultyLevel difficulty)
        {
            await taskService.UpdateDifficultyAsync(projectId, taskId, difficulty);
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
