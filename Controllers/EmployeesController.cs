using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService employeeService;

        public UsersController(IUserService employeeService)
        {
            this.employeeService = employeeService;
        }
        // GET: api/<UsersController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<List<User>> Get()
        {
            return employeeService.Get();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(ObjectId id)
        {
            var employee = employeeService.Get(id);

            if (employee == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return employee;
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User employee)
        {
            employeeService.Create(employee);

            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult Put(ObjectId id, [FromBody] User employee)
        {
            var existingUser = employeeService.Get(id);

            if (existingUser == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            employeeService.Update(id, employee);

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(ObjectId id)
        {
            var employee = employeeService.Get(id);

            if (employee == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            employeeService.Remove(employee.Id);

            return Ok($"User with Id = {id} deleted");
        }
    }
}
