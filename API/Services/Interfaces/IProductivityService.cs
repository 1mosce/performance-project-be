using PerformanceProject.Shared.Models;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;

namespace PeopleManagmentSystem_API.Services.Interfaces
{
    public interface IProductivityService
    {
        double CalculateIndividualProductivity(string projectId, string assigneeId);
        double CalculateIndividualProductivity(List<PerformanceProject.Shared.Models.Task> tasks, ProductivityWeights weights);
        double CalculateTeamProductivity(string projectId);

        double PredictIndividualProductivity(ProductivityData inputData);
        Dictionary<string, double> PredictTeamProductivity(string projectId);

    }
}
