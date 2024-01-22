using Microsoft.AspNetCore.Mvc;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        // GET: api/<EmployeesController>
        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            return employeeService.Get();
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public ActionResult<Employee> Get(string id)
        {
            var employee = employeeService.Get(id);

            if (employee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }

            return employee;
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public ActionResult<Employee> Post([FromBody] Employee employee)
        {
            employeeService.Create(employee);

            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Employee employee)
        {
            var existingEmployee = employeeService.Get(id);

            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }

            employeeService.Update(id, employee);

            return NoContent();
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var employee = employeeService.Get(id);

            if (employee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }

            employeeService.Remove(employee.Id);

            return Ok($"Employee with Id = {id} deleted");
        }
    }
}
