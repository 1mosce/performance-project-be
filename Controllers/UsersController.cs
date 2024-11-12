using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _userService.GetAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get User by Id")]
        public async Task<ActionResult<User>> Get(ObjectId id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return Ok(user);
        }

        [HttpGet("{id}/companies")]
        [SwaggerOperation(Summary = "Get User's Companies")]
        public async Task<ActionResult<List<Company>>> GetCompanies(ObjectId id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            var companies = await _userService.GetCompaniesAsync(id);
            return Ok(companies);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New User")]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a User")]
        public async Task<ActionResult> Put(ObjectId id, [FromBody] User user)
        {
            var existingUser = await _userService.GetAsync(id);

            if (existingUser == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            await _userService.UpdateAsync(id, user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a User")]
        public async Task<ActionResult> Delete(ObjectId id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            await _userService.RemoveAsync(user.Id);

            return NoContent();
        }
    }
}
