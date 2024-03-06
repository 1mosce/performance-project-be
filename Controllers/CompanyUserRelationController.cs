using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Services;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CompanyUserRelationController : ControllerBase
    {
        private readonly ICompanyUserService _сompanyUserService;

        public CompanyUserRelationController(ICompanyUserService сompanyUserService)
        {
            _сompanyUserService = сompanyUserService;
        }
        // GET: api/<UsersController>
        [HttpGet("company-user")]
        public ActionResult<List<CompanyUser>> Get()
        {
            return _сompanyUserService.Get();
        }

        // GET api/<UsersController>/5
        [HttpGet("user-companies/{id}")]
        public ActionResult<List<Company>> GetUserCompanies(ObjectId id)
        {
            var user = _сompanyUserService.GetUser(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return _сompanyUserService.GetUserCompanies(id);
        }

        [HttpGet("company-users/{id}")]
        public ActionResult<List<User>> GetCompanyUsers(ObjectId id)
        {
            var company = _сompanyUserService.GetCompany(id);

            if (company == null)
            {
                return NotFound($"Company with Id = {id} not found");
            }

            return _сompanyUserService.GetCompanyUsers(id);
        }

        [HttpPut("company-user/{companyId}/{userId}")]
        public ActionResult UpdateCompanyUserRelation(ObjectId companyId, ObjectId userId)
        {
            var company = _сompanyUserService.GetCompany(companyId);
            var user = _сompanyUserService.GetUser(userId);

            if (company == null || user == null)
            {
                return NotFound();
            }

            _сompanyUserService.Update(companyId, userId);

            return NoContent();
        }
    }
}
