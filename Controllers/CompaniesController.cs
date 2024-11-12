using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
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
        public async Task<ActionResult<Company>> Get(ObjectId id)
        {
            var company = await companyService.GetAsync(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return Ok(company);
        }

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

        [HttpPut("{companyId}/{userId}")]
        [SwaggerOperation(Summary = "Update a User's Company Membership")]
        public async Task<IActionResult> UpdateUser(ObjectId companyId, ObjectId userId)
        {
            var company = await companyService.GetAsync(companyId);
            if (company == null)
            {
                return NotFound($"Company with Id = {companyId} not found");
            }

            var userExists = await companyService.UserExistsAsync(userId);
            if (!userExists)
            {
                return NotFound($"User with Id = {userId} not found");
            }

            companyService.UpdateUserAsync(companyId, userId);

            return NoContent();
        }

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
    }
}
