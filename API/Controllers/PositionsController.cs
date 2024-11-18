using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Positions")]
        public async Task<ActionResult<List<Position>>> Get()
        {
            var positions = await _positionService.GetAsync();
            return Ok(positions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Position by Id")]
        public async Task<ActionResult<Position>> GetById(string id)
        {
            var position = await _positionService.GetAsync(id);

            if (position == null)
            {
                return NotFound($"Position with Id = {id} not found");
            }

            return Ok(position);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Position")]
        public async Task<ActionResult<Position>> Post([FromBody] Position position)
        {
            var createdPosition = await _positionService.CreateAsync(position);
            return CreatedAtAction(nameof(Get), new { id = createdPosition.Id }, createdPosition);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Position")]
        public async Task<ActionResult> Put(string id, [FromBody] Position position)
        {
            var existingPosition = await _positionService.GetAsync(id);

            if (existingPosition == null)
            {
                return NotFound($"Position with Id = {id} not found");
            }

            await _positionService.UpdateAsync(id, position);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Position")]
        public async Task<ActionResult> Delete(string id)
        {
            var position = await _positionService.GetAsync(id);

            if (position == null)
            {
                return NotFound($"Position with Id = {id} not found");
            }

            await _positionService.RemoveAsync(id);

            return NoContent();
        }
    }
}
