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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Projects")]
        public async Task<ActionResult<List<Project>>> Get()
        {
            var projects = await projectService.GetAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Project by Id")]
        public async Task<ActionResult<Project>> Get(ObjectId id)
        {
            var project = await projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            return Ok(project);
        }


        [HttpGet("{id}/tasks")]
        [SwaggerOperation(Summary = "Get Project's Tasks")]
        public async Task<ActionResult<List<Task>>> GetTasks(ObjectId id)
        {
            var project = await projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            var tasks = await projectService.GetTasksAsync(id);
            return Ok(tasks);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Project")]
        public async Task<ActionResult<Project>> Post([FromBody] Project project)
        {
            await projectService.CreateAsync(project);

            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Project")]
        public async Task<ActionResult> Put(ObjectId id, [FromBody] Project project)
        {
            var existingProject = await projectService.GetAsync(id);

            if (existingProject == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            await projectService.UpdateAsync(id, project);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Project")]
        public async Task<ActionResult> Delete(ObjectId id)
        {
            var project = await projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            await projectService.RemoveAsync(project.Id);

            return NoContent();
        }
    }
}
