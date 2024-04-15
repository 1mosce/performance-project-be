using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
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
        public ActionResult<List<Team>> Get()
        {
            return teamService.Get();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Team by Id")]
        public ActionResult<Team> Get(ObjectId id)
        {
            var team = teamService.Get(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            return team;
        }

        [HttpGet("{id}/users")]
        [SwaggerOperation(Summary = "Get Team's Users")]
        public ActionResult<List<User>> GetUsers(ObjectId id)
        {
            var team = teamService.Get(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            return teamService.GetUsers(id);
        }

        [HttpPut("{teamId}/{userId}")]
        [SwaggerOperation(Summary = "Update a User's Team Membership")]
        public ActionResult UpdateUser(ObjectId teamId, ObjectId userId, ObjectId teamRoleId)
        {
            teamService.UpdateUser(teamId, userId, teamRoleId);

            return NoContent();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Team")]
        public ActionResult<Team> Post([FromBody] Team team)
        {
            teamService.Create(team);

            return CreatedAtAction(nameof(Get), new { id = team.Id }, team);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Team")]
        public ActionResult Put(ObjectId id, [FromBody] Team team)
        {
            var existingTeam = teamService.Get(id);

            if (existingTeam == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            teamService.Update(id, team);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Team")]
        public ActionResult Delete(ObjectId id)
        {
            var team = teamService.Get(id);

            if (team == null)
            {
                return NotFound($"Team with Id = {id} not found");
            }

            teamService.Remove(team.Id);

            return Ok($"Team with Id = {id} deleted");
        }
    }
}
