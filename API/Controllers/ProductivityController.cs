using Microsoft.AspNetCore.Mvc;
using PeopleManagmentSystem_API.Services.Interfaces;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductivityController : ControllerBase
    {
        private readonly IProductivityService _productivityService;
        public ProductivityController(IProductivityService productivityService)
        {
            _productivityService = productivityService;
        }

        [HttpGet("individual/{projectId}/{assigneeId}")]
        public ActionResult<double> GetIndividualProductivity(string projectId, string assigneeId)
        {
            try
            {
                var productivity = _productivityService.CalculateIndividualProductivity(projectId, assigneeId);
                return Ok(productivity);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("team/{projectId}")]
        public ActionResult<double> GetTeamProductivity(string projectId)
        {
            try
            {
                return _productivityService.CalculateTeamProductivity(projectId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("predict/team/{projectId}")]
        public ActionResult<Dictionary<string, double>> PredictTeamProductivity(string projectId)
        {
            try
            {
                return _productivityService.PredictTeamProductivity(projectId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
