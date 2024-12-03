namespace PerformanceProject.Shared.Models
{
    public enum ProjectStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Archived
    }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Completed,
        Blocked,
    }

    public enum DifficultyLevel
    {
        Low = 1,
        Medium,
        High,
        VeryHigh
    }
}
