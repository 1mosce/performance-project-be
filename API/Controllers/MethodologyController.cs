using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodologyController : ControllerBase
    {
        private readonly IMethodologyService _methodologyService;

        public MethodologyController(IMethodologyService methodologyService)
        {
            _methodologyService = methodologyService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Methodologies")]
        public async Task<ActionResult<List<Methodology>>> Get()
        {
            var methodologies = await _methodologyService.GetAsync();
            return Ok(methodologies);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Methodology by Id")]
        public async Task<ActionResult<Methodology>> GetById(ObjectId id)
        {
            var methodology = await _methodologyService.GetAsync(id);

            if (methodology == null)
            {
                return NotFound($"Methodology with Id = {id} not found");
            }

            return Ok(methodology);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Methodology")]
        public async Task<ActionResult<Methodology>> Post([FromBody] Methodology methodology)
        {
            await _methodologyService.CreateAsync(methodology);
            return CreatedAtAction(nameof(Get), new { id = methodology.Id }, methodology);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Methodology")]
        public async Task<ActionResult> Put(ObjectId id, [FromBody] Methodology methodology)
        {
            var existingMethodology = await _methodologyService.GetAsync(id);

            if (existingMethodology == null)
            {
                return NotFound($"Methodology with Id = {id} not found");
            }

            await _methodologyService.UpdateAsync(id, methodology);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Methodology")]
        public async Task<ActionResult> Delete(ObjectId id)
        {
            var methodology = await _methodologyService.GetAsync(id);

            if (methodology == null)
            {
                return NotFound($"Methodology with Id = {id} not found");
            }

            await _methodologyService.RemoveAsync(id);
            return NoContent();
        }
    }
}
