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
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Users")]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get User by Id")]
        public ActionResult<User> Get(ObjectId id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return user;
        }

        [HttpGet("{id}/companies")]
        [SwaggerOperation(Summary = "Get User's Companies")]
        public ActionResult<List<Company>> GetCompanies(ObjectId id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return userService.GetCompanies(id);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New User")]
        public ActionResult<User> Post([FromBody] User user)
        {
            userService.Create(user);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a User")]
        public ActionResult Put(ObjectId id, [FromBody] User user)
        {
            var existingUser = userService.Get(id);

            if (existingUser == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            userService.Update(id, user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a User")]
        public ActionResult Delete(ObjectId id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            userService.Remove(user.Id);

            return Ok($"User with Id = {id} deleted");
        }
    }
}
