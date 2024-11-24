using Microsoft.AspNetCore.Mvc;
using PeopleManagmentSystem_API.Services;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly TrainingService _trainingService;

        public TrainingController()
        {
            _trainingService = new TrainingService();
        }

        [HttpPost("train")]
        public IActionResult TrainModel()
        {
            try
            {
                var metrics = _trainingService.TrainModel();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during training: {ex.Message}");
            }
        }
    }
}
