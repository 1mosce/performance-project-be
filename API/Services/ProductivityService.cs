using PeopleManagmentSystem_API.Services;
using PeopleManagmentSystem_API.Services.Interfaces;
using PerformanceProject.Shared.Models;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;
using TaskStatus = PerformanceProject.Shared.Models.TaskStatus;

namespace PerformanceProject.Services
{
    public class ProductivityService : IProductivityService
    {
        private readonly IProjectService _projectService;
        private readonly PredictionService _predictionService;


        public ProductivityService(IProjectService projectService)
        {
            _projectService = projectService;
            _predictionService = new();
        }

        public double CalculateIndividualProductivity(string projectId, string assigneeId)
        {
            var project = _projectService.GetAsync(projectId).Result;
            var tasks = project.Tasks.Where(task => task.AssigneeId == assigneeId).ToList();

            if (!tasks.Any())
            {
                throw new ArgumentException($"No tasks found for assignee with ID {assigneeId} in project {projectId}.");
            }

            return CalculateIndividualProductivity(tasks, project.Weights);
        }

        public double CalculateIndividualProductivity(List<Shared.Models.Task> tasks, ProductivityWeights weights)
        {
            double totalProductivity = 0;
            int completedTasksCount = 0;

            foreach (var task in tasks)
            {
                if (task.Status == TaskStatus.Completed)
                {
                    completedTasksCount++;

                    double taskContribution = (int)task.Difficulty * weights.TaskWeight;
                    double timeFactor = 0;
                    if (task.PlannedTime.HasValue && task.TimeSpent.HasValue)
                    {
                        double plannedHours = task.PlannedTime.Value.TotalHours;
                        double spentHours = task.TimeSpent.Value.TotalHours;

                        timeFactor = plannedHours / Math.Max(spentHours, 1) * weights.TimeWeight;
                    }
                    double qualityFactor = (task.Rating ?? 0) * weights.QualityWeight;
                    double engagementFactor = (task.Engagement ?? 0) * weights.EngagementWeight;

                    totalProductivity += taskContribution + timeFactor + qualityFactor + engagementFactor;
                }
            }

            return completedTasksCount > 0 ? totalProductivity / completedTasksCount : 0;
        }

        public double CalculateTeamProductivity(string projectId)
        {
            var project = _projectService.GetAsync(projectId).Result;
            var tasks = project.Tasks;
            if (tasks == null || !tasks.Any())
                return 0;

            var groupedTasks = project.Tasks
                .Where(task => task.Status == TaskStatus.Completed)
                .GroupBy(task => task.AssigneeId);

            var individualProductivities = groupedTasks
                .Select(group =>
                {
                    var tasks = group.ToList();
                    return new UserProductivity
                    {
                        UserId = group.Key,
                        TotalProductivity = CalculateIndividualProductivity(tasks, project.Weights)
                    };
                })
                .ToList();

            return individualProductivities.Average(p => p.TotalProductivity);
        }

        public double PredictIndividualProductivity(ProductivityData inputData)
        {
            return _predictionService.PredictProductivity(inputData);
        }

        public Dictionary<string, double> PredictTeamProductivity(string projectId)
        {
            var project = _projectService.GetAsync(projectId).Result;
            if (project == null || project.Team == null || project.Tasks == null)
            {
                throw new ArgumentException("Invalid project data.");
            }

            var predictions = new Dictionary<string, double>();
            foreach (var member in project.Team.Members)
            {
                var memberTasks = project.Tasks
                    .Where(t => t.AssigneeId == member.UserId && t.Status == TaskStatus.Completed)
                    .ToList();

                if (!memberTasks.Any())
                {
                    predictions.Add(member.UserId, 0); // Продуктивність 0
                    continue;
                }

                // Average ProductivityData
                var inputData = new ProductivityData
                {
                    TaskDifficulty = (float)memberTasks.Average(t => (float)t.Difficulty),
                    PlannedTime = (float)memberTasks.Sum(t => t.PlannedTime.Value.TotalHours),
                    RealTime = (float)memberTasks.Sum(t => t.TimeSpent.Value.TotalHours),
                    TaskRating = (float)memberTasks.Average(t => t.Rating),
                    Engagement = (float)project.Weights.EngagementWeight,
                    TasksPerPeriod = memberTasks.Count
                };

                var predicted = _predictionService.PredictProductivity(inputData);
                predictions.Add(member.UserId, predicted);
            }

            return predictions;
        }
    }
}