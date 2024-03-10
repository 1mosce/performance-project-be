﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        [HttpGet]
        public ActionResult<List<Company>> Get()
        {
            return companyService.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Company> Get(ObjectId id)
        {
            var company = companyService.Get(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return company;
        }

        [HttpGet("{id}/users")]
        public ActionResult<List<User>> GetUsers(ObjectId id)
        {
            var company = companyService.Get(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return companyService.GetUsers(id);
        }

        [HttpPut("{companyId}/{userId}")]
        public ActionResult UpdateUser(ObjectId companyId, ObjectId userId)
        {
            companyService.UpdateUser(companyId, userId);

            return NoContent();
        }

        [HttpGet("{id}/projects")]
        public ActionResult<List<Project>> GetProjects(ObjectId id)
        {
            var company = companyService.Get(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return companyService.GetProjects(id);
        }

        [HttpPost]
        public ActionResult<Company> Post([FromBody] Company company)
        {
            companyService.Create(company);

            return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public ActionResult Put(ObjectId id, [FromBody] Company company)
        {
            var existingCompany = companyService.Get(id);

            if (existingCompany == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            companyService.Update(id, company);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(ObjectId id)
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
