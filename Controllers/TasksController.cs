using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;
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
        public ActionResult<List<Task>> Get()
        {
            return taskService.Get();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Task by Id")]
        public ActionResult<Task> Get(ObjectId id)
        {
            var task = taskService.Get(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return task;
        }

        [HttpGet("{id}/productivity")]
        [SwaggerOperation(Summary = "Get Task`s Productivity")]
        public ActionResult<double> GetProductivity(ObjectId id)
        {
            var task = taskService.Get(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return taskService.GetProductivity(id);
        }


        [HttpGet("{id}/comments")]
        [SwaggerOperation(Summary = "Get Task's Comments")]
        public ActionResult<List<Comment>> GetProjects(ObjectId id)
        {
            var company = taskService.Get(id);

            if (company == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return taskService.GetComments(id);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Task")]
        public ActionResult<Task> Post([FromBody] Task task)
        {
            taskService.Create(task);

            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Task")]
        public ActionResult Put(ObjectId id, [FromBody] Task task)
        {
            var existingTask = taskService.Get(id);

            if (existingTask == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            taskService.Update(id, task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Task")]
        public ActionResult Delete(ObjectId id)
        {
            var task = taskService.Get(id);

            if (task == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            taskService.Remove(task.Id);

            return Ok($"Task with Id = {id} deleted");
        }
    }
}
