using Microsoft.AspNetCore.Mvc;
using PeopleManagmentSystem_API.Services;
using PerfomanceProject.Shared.Models.ML;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly PredictionService _predictionService;

        public PredictionController()
        {
            _predictionService = new PredictionService();
        }

        [HttpPost("predict")]
        public IActionResult PredictProductivity([FromBody] ProductivityData inputData)
        {
            try
            {
                var predictedProductivity = _predictionService.PredictProductivity(inputData);
                return Ok(new { PredictedProductivity = predictedProductivity });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during prediction: {ex.Message}");
            }
        }

        [HttpGet("evaluate")]
        public IActionResult EvaluateModel()
        {
            try
            {
                var metrics = _predictionService.EvaluateModel();

                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during model evaluation: {ex.Message}");
            }
        }

    }
}
