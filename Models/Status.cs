﻿namespace PeopleManagmentSystem_API.Models
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
        Low,
        Medium,
        High,
        VeryHigh
    }
}
