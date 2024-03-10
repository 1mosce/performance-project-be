using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;

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
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(ObjectId id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return user;
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            userService.Create(user);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpGet("{id}/companies")]
        public ActionResult<List<Company>> GetCompanies(ObjectId id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return userService.GetCompanies(id);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
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

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
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
