using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Teams")]
        public async Task<ActionResult<List<Team>>> Get()
        {
            var teams = await teamService.GetAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Team by Id")]
        public async Task<ActionResult<Team>> GetById(string id)
        {
            var team = await teamService.GetAsync(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            return Ok(team);
        }

        [HttpGet("{id}/users")]
        [SwaggerOperation(Summary = "Get Team's Users")]
        public async Task<ActionResult<List<User>>> GetUsers(string id)
        {
            var team = await teamService.GetAsync(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            var users = await teamService.GetUsersAsync(id);
            return Ok(users);
        }

        [HttpPost("{teamId}/members")]
        [SwaggerOperation(Summary = "Add a Member to a Team")]
        public async Task<IActionResult> AddMember(string teamId, string userId, string teamRoleId)
        {
            await teamService.AddMemberAsync(teamId, userId, teamRoleId);
            return NoContent();
        }

        [HttpPut("{teamId}/{userId}")]
        [SwaggerOperation(Summary = "Update a User's Team Role")]
        public async Task<ActionResult> UpdateUser(string teamId, string userId, string teamRoleId)
        {
            var teamExists = await teamService.GetAsync(teamId);
            if (teamExists == null)
            {
                return NotFound($"Team with Id = {teamId} not found");
            }

            await teamService.UpdateMemberAsync(teamId, userId, teamRoleId);
            return NoContent();
        }

        [HttpGet("{teamId}/members")]
        [SwaggerOperation(Summary = "Get all Members of a Team")]
        public async Task<ActionResult<List<TeamMember>>> GetMembers(string teamId)
        {
            try
            {
                var members = await teamService.GetMembersAsync(teamId);
                return Ok(members);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Team")]
        public async Task<ActionResult<Team>> Post([FromBody] Team team)
        {
            var createdTeam = await teamService.CreateAsync(team);
            return CreatedAtAction(nameof(Get), new { id = createdTeam.Id }, createdTeam);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Team")]
        public async Task<ActionResult> Put(string id, [FromBody] Team team)
        {
            var existingTeam = await teamService.GetAsync(id);

            if (existingTeam == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            await teamService.UpdateAsync(id, team);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Team")]
        public async Task<ActionResult> Delete(string id)
        {
            var team = await teamService.GetAsync(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            await teamService.RemoveAsync(id);

            return NoContent();
        }
    }
}
