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
        public ActionResult<List<Project>> Get()
        {
            return projectService.Get();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Project by Id")]
        public ActionResult<Project> Get(ObjectId id)
        {
            var project = projectService.Get(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            return project;
        }


        [HttpGet("{id}/tasks")]
        [SwaggerOperation(Summary = "Get Project's Tasks")]
        public ActionResult<List<Task>> GetTasks(ObjectId id)
        {
            var company = projectService.Get(id);

            if (company == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            return projectService.GetTasks(id);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Project")]
        public ActionResult<Project> Post([FromBody] Project project)
        {
            projectService.Create(project);

            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Project")]
        public ActionResult Put(ObjectId id, [FromBody] Project project)
        {
            var existingProject = projectService.Get(id);

            if (existingProject == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            projectService.Update(id, project);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Project")]
        public ActionResult Delete(ObjectId id)
        {
            var project = projectService.Get(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            projectService.Remove(project.Id);

            return Ok($"Project with Id = {id} deleted");
        }
    }
}
