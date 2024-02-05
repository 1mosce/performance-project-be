using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services.Interfaces;

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
        // GET: api/<CompanysController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<List<Company>> Get()
        {
            return companyService.Get();
        }

        // GET api/<CompanysController>/5
        [HttpGet("{id}")]
        public ActionResult<Company> Get(string id)
        {
            var company = companyService.Get(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return company;
        }

        // POST api/<CompanysController>
        [HttpPost]
        public ActionResult<Company> Post([FromBody] Company company)
        {
            companyService.Create(company);

            return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
        }

        // PUT api/<CompanysController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Company company)
        {
            var existingCompany = companyService.Get(id);

            if (existingCompany == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            companyService.Update(id, company);

            return NoContent();
        }

        // DELETE api/<CompanysController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var company = companyService.Get(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            companyService.Remove(company.Id);

            return Ok($"Company with Id = {id} deleted");
        }
    }
}
