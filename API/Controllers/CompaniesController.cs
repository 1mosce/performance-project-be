using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompaniesController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Companies")]
        public async Task<ActionResult<List<Company>>> Get()
        {
            var companies = await companyService.GetAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Company by Id")]
        public async Task<ActionResult<Company>> GetById(ObjectId id)
        {
            var company = await companyService.GetAsync(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return Ok(company);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Company")]
        public async Task<ActionResult<Company>> Post([FromBody] Company company)
        {
            await companyService.CreateAsync(company);

            return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Company")]
        public async Task<ActionResult> Put(ObjectId id, [FromBody] Company company)
        {
            var existingCompany = await companyService.GetAsync(id);

            if (existingCompany == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            await companyService.UpdateAsync(id, company);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Company")]
        public async Task<ActionResult> Delete(ObjectId id)
        {
            var company = await companyService.GetAsync(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            await companyService.RemoveAsync(company.Id);

            return NoContent();
        }

        //Users

        [HttpGet("{id}/users")]
        [SwaggerOperation(Summary = "Get Company's Users")]
        public async Task<ActionResult<List<User>>> GetUsers(ObjectId id)
        {
            var company = await companyService.GetAsync(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            var users = await companyService.GetUsersAsync(id);

            return Ok(users);
        }

        [HttpPut("{companyId}/users/{userId}")]
        [SwaggerOperation(Summary = "Add User to Company")]
        public async Task<IActionResult> AddUser(ObjectId companyId, ObjectId userId)
        {
            await companyService.AddUserAsync(companyId, userId);
            return NoContent();
        }

        [HttpDelete("{companyId}/users/{userId}")]
        [SwaggerOperation(Summary = "Remove User from Company")]
        public async Task<IActionResult> RemoveUser(ObjectId companyId, ObjectId userId)
        {
            await companyService.RemoveUserAsync(companyId, userId);
            return NoContent();
        }

        // Projects

        [HttpGet("{id}/projects")]
        [SwaggerOperation(Summary = "Get Company's Projects")]
        public async Task<ActionResult<List<Project>>> GetProjects(ObjectId id)
        {
            var company = await companyService.GetAsync(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            var projects = await companyService.GetProjectsAsync(id);

            if (projects == null || !projects.Any())
            {
                return NotFound($"No projects found for Company with Id = {id}");
            }

            return Ok(projects);
        }

        [HttpPost("{companyId}/projects/{projectId}")]
        [SwaggerOperation(Summary = "Add Project to Company")]
        public async Task<IActionResult> AddProject(ObjectId companyId, ObjectId projectId)
        {
            await companyService.AddProjectAsync(companyId, projectId);
            return Ok();
        }

        [HttpPut("{companyId}/projects")]
        [SwaggerOperation(Summary = "Update Project in Company")]
        public async Task<IActionResult> UpdateProjectAsync(ObjectId companyId, [FromBody] Project project)
        {
            await companyService.UpdateProjectAsync(companyId, project);
            return NoContent();
        }

        [HttpDelete("{companyId}/projects/{projectId}")]
        [SwaggerOperation(Summary = "Remove Project from Company")]
        public async Task<IActionResult> RemoveProjectAsync(ObjectId companyId, ObjectId projectId)
        {
            await companyService.RemoveProjectAsync(companyId, projectId);
            return NoContent();
        }



    }
}
