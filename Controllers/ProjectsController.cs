using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services.Interfaces;

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
        public ActionResult<List<Project>> Get()
        {
            return projectService.Get();
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(ObjectId id)
        {
            var project = projectService.Get(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            return project;
        }

        // POST api/<ProjectsController>
        [HttpPost]
        public ActionResult<Project> Post([FromBody] Project project)
        {
            projectService.Create(project);

            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        // PUT api/<ProjectsController>/5
        [HttpPut("{id}")]
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

        // DELETE api/<ProjectsController>/5
        [HttpDelete("{id}")]
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
