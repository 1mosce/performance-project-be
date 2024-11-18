using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamRolesController : ControllerBase
    {
        private readonly ITeamRoleService _teamRoleService;

        public TeamRolesController(ITeamRoleService teamRoleService)
        {
            _teamRoleService = teamRoleService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Team Roles")]
        public async Task<ActionResult<List<TeamRole>>> Get()
        {
            var roles = await _teamRoleService.GetAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Team Role by Id")]
        public async Task<ActionResult<TeamRole>> GetById(string id)
        {
            var role = await _teamRoleService.GetAsync(id);

            if (role == null)
            {
                return NotFound($"Team Role with Id = {id} not found");
            }

            return Ok(role);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Team Role")]
        public async Task<ActionResult<TeamRole>> Post([FromBody] TeamRole teamRole)
        {
            var createdRole = await _teamRoleService.CreateAsync(teamRole);
            return CreatedAtAction(nameof(Get), new { id = createdRole.Id }, createdRole);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Team Role")]
        public async Task<ActionResult> Put(string id, [FromBody] TeamRole teamRole)
        {
            var existingRole = await _teamRoleService.GetAsync(id);

            if (existingRole == null)
            {
                return NotFound($"Team Role with Id = {id} not found");
            }

            await _teamRoleService.UpdateAsync(id, teamRole);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Team Role")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _teamRoleService.GetAsync(id);

            if (role == null)
            {
                return NotFound($"Team Role with Id = {id} not found");
            }

            await _teamRoleService.RemoveAsync(role.SerializedId);
            return NoContent();
        }
    }
}
